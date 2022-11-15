namespace Authentication.Controllers.Models.SocialMediaModels
{
    public class Follow
    {
        public int id { get; set; }
        public int followedId { get; set; }
        public string followDate { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");

        //Connections
        public IList<FollowUserProfile>? followUserProfile { get; set; }
    }
}
