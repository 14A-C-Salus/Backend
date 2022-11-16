using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
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

        [HttpPost("follow"), Authorize]
        public async Task<IActionResult> Follow(FollowRequest request)
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

            var follow = new UserProfileToUserProfile {
                followerId = followerUserProfile.id,
                followedId = followedUserProfile.id,
                followDate = DateTime.Now.ToString("yyyy.MM.dd")
            };

            _dataContext.userProfileToUserProfile.Add(follow);
            await _dataContext.SaveChangesAsync();
            return Ok($"{followerAuth.username} started following {followedAuth.username}!");
        }
    }
}
