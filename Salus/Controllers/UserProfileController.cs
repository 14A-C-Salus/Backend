using Microsoft.AspNetCore.Mvc;
using Salus.Services;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                return Ok(_userProfileService.CreateProfile(request));
            });
        }

        [HttpPatch("modify-profile")]
        public IActionResult ModifyProfile(UserSetDatasRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.ModifyProfile(request));
            });
        }

        [HttpPatch("set-profile-picture")]
        public IActionResult SetProfilePicture(UserSetProfilePictureRequset request)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.SetProfilePicture(request));
            });
         }
    }
}
