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
        public int? oilPortionMl { get; set; }
        public List<int>? ingredientPortionGramm { get; set; }
        public string name { get; set; } = string.Empty;
        [Required]
        public UserProfile Author { get; set; } = new ();
        public List<Tag> tags { get; set; } = new ();

        //Connections
        public IList<RecipesIncludeIngredients>? ingredients { get; set; }
        public IList<UsersLikeRecipes>? usersWhoLiked { get; set; }

        public int? oilId { get; set; }
        [ForeignKey("oilId")]
        public virtual Oil? oil { get; set; }

    }
}
