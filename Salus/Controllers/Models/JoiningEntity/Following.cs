namespace Salus.Controllers.Models.JoiningEntity
{
    public class Following
    {
        public int followerId { get; set; }
        [Required]
        public UserProfile? follower { get; set; }
        public string followDate { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");
        public int followedId { get; set; }
        [Required]
        public UserProfile? followed { get; set; }
    }
}
