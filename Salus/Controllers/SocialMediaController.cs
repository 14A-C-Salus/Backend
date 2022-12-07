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
            string badRequest = _socialMediaService.CheckUnFollowFollowRequest(request);
            if (badRequest == "")
                return BadRequest(badRequest);

            return Ok(_socialMediaService.StartOrStopFollow(request).Result);
        }


        [HttpPost("write-a-comment"), Authorize]
        public IActionResult WriteComment(WriteCommentRequest request)
        {
            string badRequest = _socialMediaService.CheckWriteCommentRequest(request);

            if (badRequest != "")
                return BadRequest(badRequest);
            
            return Ok(_socialMediaService.SendComment(request).Result);
        }

        [HttpPost("delete-comment"), Authorize]
        public IActionResult DeleteComment(int commentId)
        {
            string badRequest = _socialMediaService.CheckDeleteCommentRequest(commentId);

            if (badRequest != "")
                return BadRequest(badRequest);

            return Ok(_socialMediaService.DeleteCommentById(commentId));
        }

        [HttpPost("modify-comment"), Authorize]
        public async Task<IActionResult> ModifyComment(ModifyCommentRequest request)
        {
            var auth = await _dataContext.auths.FirstAsync(a => a.email == _authService.GetEmail());

            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);
            if (userProfile == null)
                return BadRequest("You need to create a user profile first!");

            var comment = await _dataContext.comments.FirstOrDefaultAsync(c => c.id == request.commentId);

            if (comment == null)
                return BadRequest("Comment doesn't exist.");

            if (userProfile.id != comment.fromId)
                return BadRequest("You do not have permission to modify the comment.");
            comment.body = request.body;
            await _dataContext.SaveChangesAsync();
            return Ok($"{comment.id} modified by {auth.username}!");
        }

        [HttpGet("get-all-comment-by-authenticated-email"), Authorize]
        public async Task<IActionResult> GetAllComment()
        {
            var auth = await _dataContext.auths.FirstAsync(a => a.email == _authService.GetEmail());
            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);
            if (userProfile == null)
                return BadRequest("You need to create a user profile first!");
            return Ok(_dataContext.comments.Where(c => c.toId == userProfile.id).ToList());
        }
    }
}
