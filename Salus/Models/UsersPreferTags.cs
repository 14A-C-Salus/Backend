using Newtonsoft.Json;

namespace Salus.Models
{
    public class UsersPreferTags
    {
        public int userId { get; set; }
        public int tagId { get; set; }
        [Required, JsonIgnore]
        public UserProfile? user { get; set; }
        [Required, JsonIgnore]
        public Tag? tag { get; set; }
    }
}
