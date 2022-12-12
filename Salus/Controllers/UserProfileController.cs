using Microsoft.AspNetCore.Mvc;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {

        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
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
        public IActionResult SetProfilePicture(UserSetProfilePictureRequset request)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.SetProfilePicture(request).Result);
            });
         }
    }
}
