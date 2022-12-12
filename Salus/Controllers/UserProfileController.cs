using Microsoft.AspNetCore.Mvc;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(DataContext dataContext, IAuthService authService, IUserProfileService userProfileService)
        {
            _dataContext = dataContext;
            _authService = authService;
            _userProfileService = userProfileService;
        }

        [HttpPut("create-profile"), Authorize]
        public IActionResult CreateProfile(UserSetDatasRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.CreateProfile(request).Result);
            });
        }

        [HttpPost("modify-profile"), Authorize]
        public IActionResult ModifyProfile(UserSetDatasRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.ModifyProfile(request).Result);
            });
        }

        [HttpPost("set-profile-picture"), Authorize]
        public async Task<IActionResult> SetProfilePicture(UserSetProfilePictureRequset request)
        {
            var email = _authService.GetEmail();

            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == email);
            if (auth == null)
                return BadRequest("You must log in first.");
            if (auth.userProfile == null)
                return BadRequest("First set the data in \"set-data\".");

            auth.userProfile = _userProfileService.SetUserProfilePicture(request, auth.userProfile);

            await _dataContext.SaveChangesAsync();
            return Ok($"Profile picture updated. (Hair: {auth.userProfile.hairIndex}, Skin color: {auth.userProfile.skinIndex}, Eye color: {auth.userProfile.eyesIndex}, Mouth: {auth.userProfile.mouthIndex})");
        }
    }
}
