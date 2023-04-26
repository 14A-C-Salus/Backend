using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
 

namespace Salus.Models
{
    public class Recipe
    {
        public int id { get; set; }
        public int gramm { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public bool verified { get; set; } = false;
        public int timeInMinute { get; set; }
        public int? oilPortionMl { get; set; } 
        public string description { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public makeingMethodEnum method { get; set; }
        [JsonIgnore]
        public UserProfile? userProfile { get; set; } = new ();

        //Connections
        public List<RecipesIncludeIngredients>? ingredients { get; set; }
        public List<UsersLikeRecipes>? usersWhoLiked { get; set; }

        public int? oilId { get; set; }
        [ForeignKey("oilId")]
        public virtual Oil? oil { get; set; }

        //Connections
        [Required, JsonIgnore]
        public List<RecipesIncludeIngredients> recipes { get; set; } = new();

        [Required]
        public List<RecepiesHaveTags> tags { get; set; } = new();
        [JsonIgnore]
        public List<Last24h>? last24hs { get; set; }
    }
}
