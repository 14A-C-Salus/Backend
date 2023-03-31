using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Salus.Controllers.Models.RecipeModels.RecipeEnums;

namespace Salus.Controllers.Models.RecipeModels
{
    public class Recipe
    {
        public int id { get; set; }
        public int gramm { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public bool verifeid { get; set; } = false;
        public int timeInMinute { get; set; }
        public int? oilPortionMl { get; set; } 
        public string description { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public makeingMethodEnum method { get; set; }
        [Required, NotMapped]
        public UserProfile Author { get; set; } = new ();

        //Connections
        public List<RecipesIncludeIngredients> ingredients { get; set; } = new();
        public List<UsersLikeRecipes> usersWhoLiked { get; set; } = new();

        public int? oilId { get; set; }
        [ForeignKey("oilId")]
        public virtual Oil? oil { get; set; }

        //Connections
        [Required, JsonIgnore]
        public List<RecipesIncludeIngredients> recipes { get; set; } = new();

        [Required, JsonIgnore]
        public List<RecepiesHaveTags> tags { get; set; } = new();
        public Last24h? last24h { get; set; }
    }
}
