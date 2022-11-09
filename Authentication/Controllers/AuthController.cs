using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public AuthController(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthRegisterRequest request)
        {
            if (_dataContext.auths.Any(a => a.username == request.username) || _dataContext.auths.Any(a => a.email == request.email))
            {
                return BadRequest("Username or email already exists.");
            }
            CreatePasswordHash(request.password,
                out byte[] passwordHash,
                out byte[] passwordSalt);
            var auth = new Auth
            {
                username = request.username,
                email = request.email,
                passwordHash = passwordHash,
                passwordSalt = passwordSalt,
                verificationToken = CreateRandomToken()
            };
            _dataContext.auths.Add(auth);
            await _dataContext.SaveChangesAsync();
            return Ok("User successfully created!");
        }

        private string CreateRandomToken()
        {
            var randomToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(128));
            while (_dataContext.auths.Any(a => a.verificationToken == randomToken) && _dataContext.auths.Any(a => a.passwordResetToken == randomToken))
            {
                randomToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(128));
            }
            return randomToken;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthLoginRequest request)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);
            if (auth == null || !VerifyPasswordHash(request.password, auth.passwordHash, auth.passwordSalt))
                return BadRequest("Username or password is not correct!");
            if (auth.date == null)
                return BadRequest($"Not verified! Token: {auth.verificationToken}.");
            string jwt = CreateToken(auth);
            return Ok(jwt);
        }

        private string CreateToken(Auth auth)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, auth.username),
                new Claim(ClaimTypes.Email, auth.email),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Keys:JwtKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedPasswordHash.SequenceEqual(passwordHash);
            }
        }


        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.verificationToken == token);
            if (auth == null)
                return BadRequest("Invalid token!");
            auth.date = DateTime.Now;
            await _dataContext.SaveChangesAsync();
            return Ok($"{auth.username} verified!");
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required, EmailAddress] string email)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == email);
            if (auth == null)
                return BadRequest("User not found!");
            auth.passwordResetToken = CreateRandomToken();
            auth.resetTokenExpires = DateTime.Now.AddDays(1);
            await _dataContext.SaveChangesAsync();
            return Ok($"Hi {auth.username}, you may now reset your password. Token: {auth.passwordResetToken}.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(AuthResetPasswordRequest request)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.passwordResetToken == request.token);
            if (auth == null || auth.resetTokenExpires < DateTime.Now)
                return BadRequest("Invalid Token!");
            CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
            auth.passwordHash = passwordHash;
            auth.passwordSalt = passwordSalt;
            auth.passwordResetToken = null;
            auth.resetTokenExpires = null;
            await _dataContext.SaveChangesAsync();
            return Ok("Password successfully reset.");
        }
    }
}
