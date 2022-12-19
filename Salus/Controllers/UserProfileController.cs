using Microsoft.AspNetCore.Mvc;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if !DEBUG
    [Authorize]
#endif
    public class UserProfileController : Controller
    {

        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpPut("create-profile")]
        public IActionResult CreateProfile(UserSetDatasRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.CreateProfile(request).Result);
            });
        }

        [HttpPatch("modify-profile")]
        public IActionResult ModifyProfile(UserSetDatasRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.ModifyProfile(request).Result);
            });
        }

        [HttpPatch("set-profile-picture")]
        public IActionResult SetProfilePicture(UserSetProfilePictureRequset request)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.SetProfilePicture(request).Result);
            });
         }
    }
}
