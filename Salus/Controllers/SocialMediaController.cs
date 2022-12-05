using Microsoft.AspNetCore.Mvc;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;

        public SocialMediaController(DataContext dataContext, IAuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        [HttpPost("un-follow/follow"), Authorize]
        public async Task<IActionResult> UnFollowFollow(UnFollowFollowRequest request)
        {
            var followerAuth = await _dataContext.auths.FirstAsync(a => a.email == _authService.GetEmail());
            var followedAuth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);
            if (followedAuth == null)
                return BadRequest("Auth to follow doesn't exist.");

            if (followerAuth.userProfile == null)
                return BadRequest("You need to create a user profile first!");

            if (followedAuth.userProfile == null)
                return BadRequest($"{followerAuth.username} has no user profile!");


            string startStop;
            if (await _dataContext.followings
                .FirstOrDefaultAsync(f => f.followerId == followerAuth.userProfile.id && f.followedId == followedAuth.userProfile.id) != null)
            {
                _dataContext.followings.Remove(await _dataContext.followings.FirstAsync(
                    f => f.followerId == followerAuth.userProfile.id && f.followedId == followedAuth.userProfile.id));
                startStop = "stopped";
            }
            else
            {
                _dataContext.followings.Add(new Following
                {
                    followed = followedAuth.userProfile,
                    follower = followerAuth.userProfile,
                    followDate = DateTime.Now.ToString("yyyy.MM.dd")
                });
                startStop = "started";
            }

            await _dataContext.SaveChangesAsync();
            return Ok($"{followerAuth.username} {startStop} following {followedAuth.username}!");
        }


        [HttpPost("write-a-comment"), Authorize]
        public async Task<IActionResult> WriteComment(WriteCommentRequest request)
        {
            var writerAuth = await _dataContext.auths.FirstAsync(a => a.email == _authService.GetEmail());
            var toAuth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);
            if (toAuth == null)
                return BadRequest("'toAuth' doesn't exist.");

            if (writerAuth.userProfile == null)
                return BadRequest("You need to create a user profile first!");

            if (toAuth.userProfile == null)
                return BadRequest($"{toAuth.username} has no user profile!");

            var comment = new Comment
            {
                commentFrom = writerAuth.userProfile,
                commentTo = toAuth.userProfile,
                body = request.body,
                sendDate = DateTime.Now.ToString("yyyy.MM.dd")
            };

            _dataContext.comments.Add(comment);
            await _dataContext.SaveChangesAsync();
            return Ok($"{writerAuth.username} sended a comment to {toAuth.username}!");
        }

        [HttpPost("delete-comment"), Authorize]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var auth = await _dataContext.auths.FirstAsync(a => a.email == _authService.GetEmail());

            if (auth.userProfile == null)
                return BadRequest("You need to create a user profile first!");

            var comment = await _dataContext.comments.FirstOrDefaultAsync(c => c.id == commentId);

            if (comment == null)
                return BadRequest("Comment doesn't exist.");

            if (auth.userProfile.id != comment.toId)
                return BadRequest("You do not have the right to delete the comment.");
            _dataContext.comments.Remove(comment);
            await _dataContext.SaveChangesAsync();
            return Ok($"{comment.id} deleted by {auth.username}!");
        }
    }
}
