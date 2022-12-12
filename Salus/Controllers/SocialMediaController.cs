using Microsoft.AspNetCore.Mvc;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;
        private readonly ISocialMediaService _socialMediaService;

        public SocialMediaController(DataContext dataContext, IAuthService authService, ISocialMediaService socialMediaService)
        {
            _dataContext = dataContext;
            _authService = authService;
            _socialMediaService = socialMediaService;
        }

        [HttpPatch("unfollow-follow"), Authorize]
        public IActionResult UnFollowFollow(UnFollowFollowRequest request)
        {
            return this.Run(() =>
            {
                _socialMediaService.StartOrStopFollow(request);
                return Ok();
            });
        }


        [HttpPut("write-comment"), Authorize]
        public IActionResult WriteComment(WriteCommentRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_socialMediaService.SendComment(request).Result);
            });
        }

        [HttpDelete("delete-comment"), Authorize]
        public IActionResult DeleteComment(int commentId)
        {
            return this.Run(() =>
            {
                _socialMediaService.DeleteCommentById(commentId);
                return Ok();
            });
        }

        [HttpPatch("modify-comment"), Authorize]
        public IActionResult ModifyComment(ModifyCommentRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_socialMediaService.ModifyComment(request).Result);
            });
        }

        [HttpGet("get-all-comment-by-authenticated-email"), Authorize]
        public IActionResult GetAllComment()
        {
            return this.Run(() =>
            {
                return Ok(_socialMediaService.CreateCommentListByAuthenticatedEmail());
            });
        }
    }
}
