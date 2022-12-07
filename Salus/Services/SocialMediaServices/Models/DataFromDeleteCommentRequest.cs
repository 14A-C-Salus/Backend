namespace Salus.Services.SocialMediaServices.Models
{
    public class DataFromDeleteCommentRequest
    {
        public UserProfile? userProfile { get; set; }
        public Comment? comment { get; set; }
    }
}
