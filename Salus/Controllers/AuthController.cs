using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Salus.WebAPI;
using System.ComponentModel.DataAnnotations;
namespace Salus.Controllers
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

        [HttpPut("register")]
        public IActionResult Register(AuthRegisterRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_authService.Register(request).Result);
            });
        }
        [HttpPost("login")]
        public IActionResult Login(AuthLoginRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_authService.Login(request).Result);
            });

        }
        [HttpPatch("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.verificationToken == token);
            if (auth == null)
                return BadRequest("Invalid token!");

            auth.date = DateTime.Now;
            await _dataContext.SaveChangesAsync();
            return Ok($"{auth.username} verified!");
        }
        [HttpPatch("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required, EmailAddress] string email)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == email);
            if (auth == null)
                return BadRequest("User not found!");

            _authService.SetTokenAndExpires(auth);

            await _dataContext.SaveChangesAsync();
            return Ok($"Hi {auth.username}, you may now reset your password. Token: {auth.passwordResetToken}.");
        }

        [HttpPatch("reset-password")]
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
