namespace Salus.Services.SocialMediaServices
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly DataContext _dataContext;
        private readonly IGenericServices<Comment> _crudComment;
        public SocialMediaService(DataContext dataContext, IGenericServices<Comment> crudComment)
        {
            _dataContext = dataContext;
            _crudComment = crudComment;
        }


        //public methods
        public async Task<Comment> ModifyComment(ModifyCommentRequest request)
        {
            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == _crudComment.GetAuthId());
            if (userProfile == null)
                throw new Exception("You need to create a user profile first!");

            var comment = await _dataContext.comments.FirstOrDefaultAsync(c => c.id == request.commentId);

            if (comment == null)
                throw new Exception("Comment doesn't exist.");

            if (userProfile.id != comment.fromId)
                throw new Exception("You do not have permission to modify the comment.");
            comment.body = request.body;
            await _dataContext.SaveChangesAsync();
            return comment;
        }


        public List<Comment> CreateCommentListByAuthenticatedEmail()
        {
            var userProfile = GetAuthenticatedAuthUserProfile().Result;

            List<Comment> comments = _dataContext.comments.Where(c => c.toId == userProfile.id).ToList();
            return comments;
        }


        public async void DeleteCommentById(int commentId)
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
       }

        public async void StartOrStopFollow(UnFollowFollowRequest request)
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

            if (follow != null)
            {
                _dataContext.followings.Remove(follow);
            }
            else
            {
                _dataContext.followings.Add(new Following
                {
                    followed = followedUserProfile,
                    follower = followerUserProfile,
                    followDate = DateTime.Now.ToString("yyyy.MM.dd")
                });
            }


            await _dataContext.SaveChangesAsync();
       }

        public async Task<Comment> SendComment(WriteCommentRequest request)
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
            return comment;
      }

        //private methods
        private async Task<UserProfile> GetAuthenticatedAuthUserProfile()
        {
            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == _authService.GetAuthId());
            if (userProfile == null)
                throw new Exception("You need to create a user profile first!");

            return userProfile;
        }
    }
}
