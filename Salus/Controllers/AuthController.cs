using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Salus.Services;
using Salus.WebAPI;
using System.ComponentModel.DataAnnotations;
namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPut("register")]
        public IActionResult Register(AuthRegisterRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_authService.Register(request));
            });
        }
        [HttpPost("asdasd")]
        public List<string> asdasd(AuthLoginRequest request)
        {
            List<string> asd = new();
            new StreamReader("./Templates/badwords.txt").ReadToEnd().Split("\n").ToList().ForEach(x => asd.Add(x.Trim()));
            return asd;
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
        public IActionResult Verify(string token)
        {
            return this.Run(() =>
            { 
                return Ok(_authService.Verify(token).Result);
            });
        }
        [HttpPatch("forgot-password")]
        public IActionResult ForgotPassword([Required, EmailAddress] string email)
        {
            return this.Run(() =>
            {
                return Ok(_authService.ForgotPassword(email).Result);
            });

        }
        [HttpPatch("reset-password")]
        public IActionResult ResetPassword(AuthResetPasswordRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_authService.ResetPassword(request).Result);
            });
        }
    }
}
