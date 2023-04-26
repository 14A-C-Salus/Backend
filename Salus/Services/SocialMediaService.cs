using Salus.Exceptions;

namespace Salus.Services.SocialMediaServices
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly DataContext _dataContext;
        private readonly GenericService<Comment> _genericServicesComment;
        public SocialMediaService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _genericServicesComment = new(dataContext, httpContextAccessor);
        }


        //public methods
        public async Task<Comment> ModifyComment(ModifyCommentRequest request)
        {
            var userProfile = _genericServicesComment.GetAuthenticatedUserProfile();

            var comment = _genericServicesComment.Read(request.commentId);

            if (comment == null)
                throw new ECommentNotFound();

            if (userProfile.id != comment.fromId)
                throw new EPermissionDenied();
            comment.body = request.body;
            await _dataContext.SaveChangesAsync();
            return comment;
        }

        public List<Comment> CreateCommentListByUserprofileId(int userprofileId)
        {
            var userProfile = _dataContext.Set<UserProfile>().FirstOrDefault(u => u.id == userprofileId );
            if (userProfile == null)
            {
                throw new EUserNotFound();
            }
            List<Comment> comments = _dataContext.Set<Comment>().Where(c => c.toId == userProfile.id).ToList();
            return comments;
        }
        public List<Comment> CreateCommentListByAuthenticatedEmail()
        {
            var userProfile = _genericServicesComment.GetAuthenticatedUserProfile();

            List<Comment> comments = _dataContext.Set<Comment>().Where(c => c.toId == userProfile.id).ToList();
            return comments;
        }


        public void DeleteCommentById(int commentId)
        {
            var userProfile = _genericServicesComment.GetAuthenticatedUserProfile();


            var comment = _genericServicesComment.Read(commentId);
            if (comment == null)
                throw new ECommentNotFound();

            if (userProfile.id != comment.toId && userProfile.id != comment.fromId)
                throw new EPermissionDenied();

            _genericServicesComment.Delete(comment);
       }

        public void StartOrStopFollow(UnFollowFollowRequest request)
        {
            var followedAuth = _dataContext.Set<Auth>().FirstOrDefault(a => a.id == request.id);
            if (followedAuth == null)
                throw new EAuthNotFound();

            var followerUserProfile = _genericServicesComment.GetAuthenticatedUserProfile();
            if (followerUserProfile == null)
                throw new EUserProfileNotFound();

            var followedUserProfile = _dataContext.Set<UserProfile>().FirstOrDefault(u => u.authOfProfileId == followedAuth.id);
            if (followedUserProfile == null)
                throw new EUserProfileNotFound();

            if (followedUserProfile.id == followerUserProfile.id)
                throw new ESelfFollow();

            var follow = _dataContext.Set<Following>()
                .FirstOrDefault(f => f.followerId == followerUserProfile.id && f.followedId == followedUserProfile.id);

            if (follow != null)
            {
                _dataContext.Set<Following>().Remove(follow);
            }
            else
            {
                _dataContext.Set<Following>().Add(new Following
                {
                    followed = followedUserProfile,
                    follower = followerUserProfile,
                    followDate = DateTime.Now.ToString("yyyy.MM.dd")
                });
            }

            _dataContext.SaveChanges();
       }

        public async Task<Comment> SendComment(WriteCommentRequest request)
        {
            var toAuth = await _dataContext.Set<Auth>().FirstOrDefaultAsync(a => a.email == request.email);
            if (toAuth == null)
                throw new EAuthNotFound();

            var toUserProfile = await _dataContext.Set<UserProfile>().FirstOrDefaultAsync(u => u.authOfProfileId == toAuth.id);
            
            var writerUserProfile = _genericServicesComment.GetAuthenticatedUserProfile();
            if (writerUserProfile == null)
                throw new EUserProfileNotFound();

            if (toUserProfile == null)
                throw new EUserProfileNotFound();

            var comment = new Comment
            {
                commentFrom = writerUserProfile,
                commentTo = toUserProfile,
                body = request.body,
                sendDate = DateTime.Now.ToString("yyyy.MM.dd")
            };

            _genericServicesComment.Create(comment);
            return comment;
      }
    }
}
