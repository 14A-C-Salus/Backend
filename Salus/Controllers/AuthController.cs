using Salus.WebAPI;
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

        [HttpGet("get-auth")]
        public IActionResult GetAuth(int authId)
        {
            return this.Run(() =>
            {
                return Ok(_authService.GetAuth(authId));
            });
        }

        [HttpGet("get-userprofile")]
        public IActionResult GetUserProfile(int authId)
        {
            return this.Run(() =>
            {
                return Ok(_authService.GetUserProfile(authId));
            });
        }

        [HttpPut("register")]
        public IActionResult Register(AuthRegisterRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_authService.Register(request));
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
        public IActionResult ResetPassword(AuthResetPasswordRequest request, [FromQuery] string token)
        {
            return this.Run(() =>
            {
                return Ok(_authService.ResetPassword(request, token).Result);
            });
        }
    }
}
