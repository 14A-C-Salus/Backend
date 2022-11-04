using Authentication.Controllers.Models.AuthModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly DataContext _dataContext;
        public AuthController(DataContext dataContext)
        {
            _dataContext = dataContext;
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
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthLoginRequest request)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);
            if (auth == null || !VerifyPasswordHash(request.password, auth.passwordHash, auth.passwordSalt))
                return BadRequest("Username or password is not correct!");
            if (auth.date == null)
                return BadRequest("Not verified!");
            return Ok($"Welcome back, {auth.username}!");
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
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == email);
            if (auth == null)
                return BadRequest("User not found!");
            auth.passwordResetToken = CreateRandomToken();
            auth.resetTokenExpires = DateTime.Now.AddDays(1);
            await _dataContext.SaveChangesAsync();
            return Ok("You may now reset your password.");
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
