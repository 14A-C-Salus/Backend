using Microsoft.AspNetCore.Mvc;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if !DEBUG
    [Authorize]
#endif
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

        [HttpPatch("unfollow-follow")]
        public IActionResult UnFollowFollow(UnFollowFollowRequest request)
        {
            return this.Run(() =>
            {
                _socialMediaService.StartOrStopFollow(request);
                return Ok();
            });
        }


        [HttpPut("write-comment")]
        public IActionResult WriteComment(WriteCommentRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_socialMediaService.SendComment(request).Result);
            });
        }

        [HttpDelete("delete-comment")]
        public IActionResult DeleteComment(int commentId)
        {
            return this.Run(() =>
            {
                _socialMediaService.DeleteCommentById(commentId);
                return Ok();
            });
        }

        [HttpPatch("modify-comment")]
        public IActionResult ModifyComment(ModifyCommentRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_socialMediaService.ModifyComment(request).Result);
            });
        }

        [HttpGet("get-all-comment-by-authenticated-email")]
        public IActionResult GetAllComment()
        {
            return this.Run(() =>
            {
                return Ok(_socialMediaService.CreateCommentListByAuthenticatedEmail());
            });
        }
    }
}
