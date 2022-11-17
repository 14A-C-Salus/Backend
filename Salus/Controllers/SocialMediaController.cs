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
                return BadRequest("You must log in first.");

            var followerUserProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == followerAuth.id);
            if (followerUserProfile == null)
                return BadRequest("You need to create a user profile first!");

            var followedUserProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == followedAuth.id);
            if (followedUserProfile == null)
                return BadRequest($"{followerAuth.username} has no user profile!");


            string startStop;
            if (await _dataContext.userProfileToUserProfile.FirstOrDefaultAsync(f => f.followerId == followerUserProfile.id && f.followedId == followedUserProfile.id) != null)
            {
                _dataContext.userProfileToUserProfile.Remove(await _dataContext.userProfileToUserProfile.FirstAsync(
                    f => f.followerId == followerUserProfile.id && f.followedId == followedUserProfile.id));
                startStop = "stopped";
            }
            else
            {
                _dataContext.userProfileToUserProfile.Add(new UserProfileToUserProfile
                {
                    followedId = followedUserProfile.id,
                    followerId = followerUserProfile.id,
                    followDate = DateTime.Now.ToString("yyyy.MM.dd")
                });
                startStop = "started";
            }

            await _dataContext.SaveChangesAsync();
            return Ok($"{followerAuth.username} {startStop} following {followedAuth.username}!");
        }
    }
}
