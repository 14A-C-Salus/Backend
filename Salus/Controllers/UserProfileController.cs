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
        [AllowAnonymous]
        [HttpGet("read-profile")]
        public IActionResult ReadProfile(int id)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.GetProfile(id));
            });
        }

        [HttpGet("get-recommended-diets")]
        public IActionResult GetRecommendedDiets()
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.GetRecommendedDiets());
            });
        }
        [HttpGet("get-userprofiles-by-name")]
        public IActionResult GetUserprofilesByName(string name)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.GetUserprofilesByName(name));
            });
        }
        [HttpPatch("add-diet")]
        public IActionResult AddDiet(int dietId)
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.AddDiet(dietId));
            });
        }

        [HttpPatch("remove-diet")]
        public IActionResult RemoveDiet()
        {
            return this.Run(() =>
            {
                return Ok(_userProfileService.RemoveDiet());
            });
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
