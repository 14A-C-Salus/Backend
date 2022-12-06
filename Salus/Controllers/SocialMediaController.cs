using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salus.Controllers.Models.AuthModels;

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
            var followerUserProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == followerAuth.id);
            var followedUserProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == followedAuth.id);

            if (followerUserProfile == null)
                return BadRequest("You need to create a user profile first!");

            if (followedUserProfile == null)
                return BadRequest($"{followerAuth.username} has no user profile!");


            string startStop;
            if (await _dataContext.followings
                .FirstOrDefaultAsync(f => f.followerId == followerUserProfile.id && f.followedId == followedUserProfile.id) != null)
            {
                _dataContext.followings.Remove(await _dataContext.followings.FirstAsync(
                    f => f.followerId == followerUserProfile.id && f.followedId == followedUserProfile.id));
                startStop = "stopped";
            }
            else
            {
                _dataContext.followings.Add(new Following
                {
                    followed = followedUserProfile,
                    follower = followerUserProfile,
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
            var writerUserProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == writerAuth.id);
            var toUserProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == toAuth.id);
            if (writerUserProfile == null)
                return BadRequest("You need to create a user profile first!");

            if (toUserProfile == null)
                return BadRequest($"{toAuth.username} has no user profile!");

            var comment = new Comment
            {
                commentFrom = writerUserProfile,
                commentTo = toUserProfile,
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

            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);
            if (userProfile == null)
                return BadRequest("You need to create a user profile first!");

            var comment = await _dataContext.comments.FirstOrDefaultAsync(c => c.id == commentId);

            if (comment == null)
                return BadRequest("Comment doesn't exist.");

            if (userProfile.id != comment.toId && userProfile.id != comment.fromId)
                return BadRequest("You do not have the right to delete the comment.");
            _dataContext.comments.Remove(comment);
            await _dataContext.SaveChangesAsync();
            return Ok($"{comment.id} deleted by {auth.username}!");
        }


        [HttpPost("get-all-comment-by-authenticated-email"), Authorize]
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
