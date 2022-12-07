namespace Salus.Services.SocialMediaServices.Models
{
    public class DataFromCommentRequest
    {
        public Auth? toAuth { get; set; }
        public UserProfile? writerUserProfile { get; set; }
        public UserProfile? toUserProfile { get; set; }
    }
}
