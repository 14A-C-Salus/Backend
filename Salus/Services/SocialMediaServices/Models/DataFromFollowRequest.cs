namespace Salus.Services.SocialMediaServices.Models
{
    public class DataFromFollowRequest
    {
        public Auth? followedAuth { get; set; }
        public UserProfile? followerUserProfile { get; set; }
        public UserProfile? followedUserProfile { get; set; }
    }
}
