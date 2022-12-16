namespace Salus.Controllers.Models.JoiningEntity
{
    public class UsersLikeRecipes
    {
        public DateTime date { get; set; }
        public int userId { get; set; }
        public UserProfile user { get; set; }
        public int recipeId { get; set; }
        public Recipe recipe { get; set; }
    }
}
