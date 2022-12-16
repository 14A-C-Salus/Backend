using System.ComponentModel.DataAnnotations.Schema;

namespace Salus.Controllers.Models.RecipeModels
{
    public class Recipe
    {
        public int id { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public string name { get; set; } = string.Empty;
        public bool verifeid { get; set; } = false;
        [Required]
        public UserProfile Author { get; set; } = new ();
        public List<int> tagIds { get; set; } = new ();

        //Connections
        [Required]
        public List<int> foodIds { get; set; } = new ();
        public List<RecipesIncludeIngredients> ingredients { get; set; }
        public List<UsersLikeRecipes> usersWhoLiked { get; set; }

        public int? oilId { get; set; }
        [ForeignKey("oilId")]
        public virtual Oil? oil { get; set; }
    }
}
