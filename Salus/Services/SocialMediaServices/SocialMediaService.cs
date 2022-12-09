namespace Salus.Services.SocialMediaServices
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;
        public SocialMediaService(DataContext dataContext, IAuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }


        //public methods
        public async Task<string> ModifyComment(ModifyCommentRequest request)
        {
            var auth = await _dataContext.auths.FirstAsync(a => a.email == _authService.GetEmail());

            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);
            if (userProfile == null)
                throw new Exception("You need to create a user profile first!");

            var comment = await _dataContext.comments.FirstOrDefaultAsync(c => c.id == request.commentId);

            if (comment == null)
                throw new Exception("Comment doesn't exist.");

            if (userProfile.id != comment.fromId)
                throw new Exception("You do not have permission to modify the comment.");
            comment.body = request.body;
            await _dataContext.SaveChangesAsync();
            return $"{comment.id} modified by {auth.username}!";
        }


        public List<Comment> CreateCommentListByAuthenticatedEmail()
        {
            var userProfile = GetAuthenticatedAuthUserProfile().Result;

            List<Comment> comments = _dataContext.comments.Where(c => c.toId == userProfile.id).ToList();
            return comments;
        }


        public async Task<string> DeleteCommentById(int commentId)
        {
            var userProfile = GetAuthenticatedAuthUserProfile().Result;
            if (userProfile == null)
                throw new Exception("You need to create a user profile first!");

            var comment = await _dataContext.comments.FirstOrDefaultAsync(c => c.id == commentId);
            if (comment == null)
                throw new Exception ("Comment doesn't exist.");

            if (userProfile.id != comment.toId && userProfile.id != comment.fromId)
                throw new Exception("You do not have permission to delete the comment.");

            _dataContext.comments.Remove(comment);
            await _dataContext.SaveChangesAsync();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return $"{comment.id} deleted by {userProfile.auth.username}!";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task<string> StartOrStopFollow(UnFollowFollowRequest request)
        {
            var followedAuth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);
            if (followedAuth == null)
                throw new Exception("Auth to follow doesn't exist.");

            var followerUserProfile = GetAuthenticatedAuthUserProfile().Result;
            if (followerUserProfile == null)
                throw new Exception("You need to create a user profile first!");

            var followedUserProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == followedAuth.id);
            if (followedUserProfile == null)
                throw new Exception($"{followedAuth.username} has no user profile!");

            if (followedUserProfile.id == followerUserProfile.id)
                throw new Exception($"You can't follow yourself.");

            var follow = await _dataContext.followings
                .FirstOrDefaultAsync(f => f.followerId == followerUserProfile.id && f.followedId == followedUserProfile.id);

            string startStop;

            if (follow != null)
            {
                _dataContext.followings.Remove(follow);
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

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return $"{followerUserProfile.auth.username} {startStop} following {followedAuth.username}!";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task<string> SendComment(WriteCommentRequest request)
        {
            var toAuth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);
            if (toAuth == null)
                throw new Exception("'toAuth' doesn't exist.");

            var toUserProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == toAuth.id);
            
            var writerUserProfile = GetAuthenticatedAuthUserProfile().Result;
            if (writerUserProfile == null)
                throw new Exception("You need to create a user profile first!");

            if (toUserProfile == null)
                throw new Exception($"{toAuth.username} has no user profile!");

            var comment = new Comment
            {
                commentFrom = writerUserProfile,
                commentTo = toUserProfile,
                body = request.body,
                sendDate = DateTime.Now.ToString("yyyy.MM.dd")
            };

            _dataContext.comments.Add(comment);
            await _dataContext.SaveChangesAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return $"{writerUserProfile.auth.username} sended a comment to {toUserProfile.auth.username}!";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        //private methods
        private async Task<UserProfile> GetAuthenticatedAuthUserProfile()
        {
            var auth = await _dataContext.auths.FirstAsync(a => a.email == _authService.GetEmail());

            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);
            if (userProfile == null)
                throw new Exception("You need to create a user profile first!");

            return userProfile;
        }
    }
}
