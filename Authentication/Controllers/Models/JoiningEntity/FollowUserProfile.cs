namespace Authentication.Controllers.Models.JoiningEntity
{
    public class FollowUserProfile
    {
        public int followId { get; set; }
        public Follow? follow { get; set; }

        public int userProfileId { get; set; }
        public UserProfile? userProfile { get; set; }
    }
}
