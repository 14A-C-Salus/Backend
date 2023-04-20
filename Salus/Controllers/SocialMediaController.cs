using Microsoft.AspNetCore.Mvc;
using Salus.Services;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SocialMediaController : Controller
    {
        private readonly ISocialMediaService _socialMediaService;

        public SocialMediaController(ISocialMediaService socialMediaService)
        {
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
        [HttpGet("get-all-comment-by-userprofile-id")]
        public IActionResult GetAllCommentByUserprofileId(int userprofileId)
        {
            return this.Run(() =>
            {
                return Ok(_socialMediaService.CreateCommentListByUserprofileId(userprofileId));
            });
        }
    }
}
