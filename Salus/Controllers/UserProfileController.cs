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

            bool isNew = auth.userProfile == null;

            auth.userProfile = _userProfileService.SetUserProfileData(request, auth.userProfile, auth);

            if (isNew)
                _dataContext.userProfiles.Add(auth.userProfile);

            await _dataContext.SaveChangesAsync();
            return Ok($"{auth.username}'s data saved. Gender: {auth.userProfile.gender}. Goal Weight: {auth.userProfile.goalWeight}.");
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
