using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("un-follow/follow"), Authorize]
        public IActionResult UnFollowFollow(UnFollowFollowRequest request)
        {
            try
            {
                return Ok(_socialMediaService.StartOrStopFollow(request).Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("write-a-comment"), Authorize]
        public IActionResult WriteComment(WriteCommentRequest request)
        {
            try
            {
                return Ok(_socialMediaService.SendComment(request).Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete-comment"), Authorize]
        public IActionResult DeleteComment(int commentId)
        {
            try
            {
                return Ok(_socialMediaService.DeleteCommentById(commentId).Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("modify-comment"), Authorize]
        public IActionResult ModifyComment(ModifyCommentRequest request)
        {
            try
            {
                return Ok(_socialMediaService.ModifyComment(request).Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-all-comment-by-authenticated-email"), Authorize]
        public IActionResult GetAllComment()
        {
            try
            {
                return Ok(_socialMediaService.CreateCommentListByAuthenticatedEmail());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
