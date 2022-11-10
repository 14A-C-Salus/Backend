using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        public AuthController(DataContext dataContext, IConfiguration configuration, IAuthService authService)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthRegisterRequest request)
        {
            if (_dataContext.auths.Any(a => a.username == request.username) || _dataContext.auths.Any(a => a.email == request.email))
                return BadRequest("Username or email already exists.");

            var auth = _authService.NewAuth(request);
            _dataContext.auths.Add(auth);
            _authService.SendToken(auth);
            await _dataContext.SaveChangesAsync();
            return Ok("User successfully created!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthLoginRequest request)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);

            if (auth == null || !_authService.VerifyPasswordHash(request.password, auth.passwordHash, auth.passwordSalt))
                return BadRequest("Username or password is not correct!");

            if (auth.date == null)
                return BadRequest($"Not verified! Token: {auth.verificationToken}.");

            string jwt = _authService.CreateToken(auth);
            return Ok(jwt);
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

            _authService.SetTokenAndExpires(auth);

            await _dataContext.SaveChangesAsync();
            return Ok($"Hi {auth.username}, you may now reset your password. Token: {auth.passwordResetToken}.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(AuthResetPasswordRequest request)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.passwordResetToken == request.token);
            if (auth == null || auth.resetTokenExpires < DateTime.Now)
                return BadRequest("Invalid Token!");

            _authService.UpdateAuthResetPasswordData(request.password, auth);

            await _dataContext.SaveChangesAsync();
            return Ok("Password successfully reset.");
        }

    }
}
