using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("set-data"), Authorize]
        public async Task<IActionResult> SetData(UserSetDatasRequest request)
        {
            var email = _authService.GetEmail();

            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == email);
            if (auth == null)
                return BadRequest("You must log in first.");

            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);

            bool isNew = userProfile == null;

            userProfile = _userProfileService.SetUserProfileData(request, userProfile, auth.id);

            if (isNew)
                _dataContext.userProfiles.Add(userProfile);

            await _dataContext.SaveChangesAsync();
            return Ok($"{auth.username}'s data saved. Gender: {userProfile.gender}. Goal Weight: {userProfile.goalWeight}.");
        }

        [HttpPost("set-profile-picture"), Authorize]
        public async Task<IActionResult> SetProfilePicture(UserSetProfilePictureRequset request)
        {
            var email = _authService.GetEmail();

            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == email);
            if (auth == null)
                return BadRequest("You must log in first.");

            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);
            if (userProfile == null)
                return BadRequest("First set the data in \"set-data\".");

            userProfile = _userProfileService.SetUserProfilePicture(request, userProfile);

            await _dataContext.SaveChangesAsync();
            return Ok($"Profile picture updated. (Hair: {userProfile.hairIndex}, Skin color: {userProfile.skinIndex}, Eye color: {userProfile.eyesIndex}, Mouth: {userProfile.mouthIndex})");
        }
    }
}
