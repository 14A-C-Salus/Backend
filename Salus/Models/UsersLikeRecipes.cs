using Newtonsoft.Json;

namespace Salus.Models
{
    public class UsersLikeRecipes
    {
        public DateTime date { get; set; }
        public int userId { get; set; }
        [Required, JsonIgnore]
        public UserProfile? user { get; set; }
        public int recipeId { get; set; }
        [Required, JsonIgnore]
        public Recipe? recipe { get; set; }
    }
}
