using Salus.Services.SocialMediaServices.Models;

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
        public List<Comment> CreateCommentListByAuthenticatedEmail()
        {
            var checkResult = CheckAuthenticatedAuthHasUserProfile();
            if (checkResult != "")
                throw new Exception(checkResult);

            var userProfile = GetAuthenticatedAuthUserProfile().Result;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            List<Comment> comments = _dataContext.comments.Where(c => c.toId == userProfile.id).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return comments;
        }

        public string CheckAuthenticatedAuthHasUserProfile()
        {
            var userProfile = GetAuthenticatedAuthUserProfile().Result;
            return userProfile == null ? "" : "You need to create a user profile first!";
        }

        public async Task<string> DeleteCommentById(int commentId)
        {
            string checkResult = CheckDeleteCommentRequest(commentId);

            if (checkResult != "")
                throw new Exception(checkResult);

            var data = GetDataFromDeleteCommentRequest(commentId).Result;

#pragma warning disable CS8604 // Possible null reference argument.
            _dataContext.comments.Remove(data.comment);
#pragma warning restore CS8604 // Possible null reference argument.
            await _dataContext.SaveChangesAsync();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return $"{data.comment.id} deleted by {data.userProfile.auth.username}!";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public string CheckDeleteCommentRequest(int commentId)
        {
            var data = GetDataFromDeleteCommentRequest(commentId).Result;

            var userProfile = data.userProfile;
            if (userProfile == null)
                return "You need to create a user profile first!";

            var comment = data.comment;

            if (comment == null)
                return "Comment doesn't exist.";

            if (userProfile.id != comment.toId && userProfile.id != comment.fromId)
                return "You do not have permission to delete the comment.";

            return "";
        }

        public async Task<string> StartOrStopFollow(UnFollowFollowRequest request)
        {
            var checkResult = CheckUnFollowFollowRequest(request);
            if (checkResult != "")
                throw new Exception(checkResult);

            var data = GetDataFromFollowRequest(request).Result;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var follow = await _dataContext.followings
                .FirstOrDefaultAsync(f => f.followerId == data.followerUserProfile.id && f.followedId == data.followedUserProfile.id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

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
                    followed = data.followedUserProfile,
                    follower = data.followerUserProfile,
                    followDate = DateTime.Now.ToString("yyyy.MM.dd")
                });
                startStop = "started";
            }


            await _dataContext.SaveChangesAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return $"{data.followerUserProfile.auth.username} {startStop} following {data.followedAuth.username}!";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public string CheckUnFollowFollowRequest(UnFollowFollowRequest request)
        {
            var data = GetDataFromFollowRequest(request).Result;

            var followedAuth = data.followedAuth;
            if (followedAuth == null)
                return "Auth to follow doesn't exist.";
            var followerUserProfile = data.followerUserProfile;
            var followedUserProfile = data.followedUserProfile;

            if (followerUserProfile == null)
                return "You need to create a user profile first!";

            if (followedUserProfile == null)
                return $"{followedAuth.username} has no user profile!";
            return "";
        }

        public async Task<string> SendComment(WriteCommentRequest request)
        {
            var exception = CheckWriteCommentRequest(request);
            if (exception != "")
                throw new Exception(exception);

            var data = GetDataFromCommentRequest(request).Result;

            var comment = new Comment
            {
                commentFrom = data.writerUserProfile,
                commentTo = data.toUserProfile,
                body = request.body,
                sendDate = DateTime.Now.ToString("yyyy.MM.dd")
            };

            _dataContext.comments.Add(comment);
            await _dataContext.SaveChangesAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return $"{data.writerUserProfile.auth.username} sended a comment to {data.toUserProfile.auth.username}!";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public string CheckWriteCommentRequest(WriteCommentRequest request)
        {
            var data = GetDataFromCommentRequest(request).Result;
            var toAuth = data.toAuth;
            if (toAuth == null)
                return "'toAuth' doesn't exist.";
            var writerUserProfile = data.writerUserProfile;
            var toUserProfile = data.toUserProfile;
            if (writerUserProfile == null)
                return "You need to create a user profile first!";

            if (toUserProfile == null)
                return $"{toAuth.username} has no user profile!";
            return "";
        }

        //private methods

        private async Task<DataFromCommentRequest> GetDataFromCommentRequest(WriteCommentRequest request)
        {
            var toAuth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);
            var writerUserProfile = GetAuthenticatedAuthUserProfile().Result;
            var toUserProfile =
                toAuth == null ? null
                : await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == toAuth.id);
            return new DataFromCommentRequest()
            {
                toAuth = toAuth,
                writerUserProfile = writerUserProfile,
                toUserProfile = toUserProfile
            };
        }

        private async Task<DataFromFollowRequest> GetDataFromFollowRequest(UnFollowFollowRequest request)
        {
            var followedAuth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);
            var followerUserProfile = GetAuthenticatedAuthUserProfile().Result;
            var followedUserProfile =
                followedAuth == null ? null
                : await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == followedAuth.id);
            return new DataFromFollowRequest()
            {
                followedAuth = followedAuth,
                followerUserProfile = followerUserProfile,
                followedUserProfile = followedUserProfile
            };
        }

        private async Task<DataFromDeleteCommentRequest> GetDataFromDeleteCommentRequest(int commentId)
        {
            var userProfile = GetAuthenticatedAuthUserProfile().Result;
            var comment = await _dataContext.comments.FirstOrDefaultAsync(c => c.id == commentId);
            return new DataFromDeleteCommentRequest()
            {
                userProfile = userProfile,
                comment = comment
            };
        }
        private async Task<UserProfile?> GetAuthenticatedAuthUserProfile()
        {
            var auth = await _dataContext.auths.FirstAsync(a => a.email == _authService.GetEmail());
            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);
            return userProfile;
        }
    }
}
