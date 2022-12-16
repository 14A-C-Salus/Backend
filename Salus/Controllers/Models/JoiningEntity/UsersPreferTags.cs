namespace Salus.Controllers.Models.JoiningEntity
{
    public class UsersPreferTags
    {
        public int userId { get; set; }
        public UserProfile user { get; set; }
        public int tagId { get; set; }
        public Tag tag { get; set; }
    }
}
