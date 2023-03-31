using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Salus.Controllers.Models.FoodModels.FoodEnums;

namespace Salus.Controllers.Models.TagModels
{
    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        [NotMapped, JsonIgnore]
        public Recipe? recipe { get; set; }
        public foodPropertiesEnum? foodProperty { get; set; }
        public int? min { get; set; }
        public int? max { get; set; }
        [NotMapped]
        public bool recommend { 
            get 
            {
                if (recipe == null)
                    return false;
                switch (foodProperty)
                {
                    case foodPropertiesEnum.protein :
                        return recipe.protein > min && recipe.protein < max;
                    case foodPropertiesEnum.fat:
                        return recipe.fat > min && recipe.fat < max;
                    case foodPropertiesEnum.carbohydrate:
                        return recipe.carbohydrate > min && recipe.carbohydrate < max;
                    case foodPropertiesEnum.kcal:
                        return recipe.kcal > min && recipe.kcal < max;
                    default:
                        return false;
                }
            }
        }

        //Connection
        [JsonIgnore, Required]
        public List<UsersPreferTags> usersWhoPrefer { get; set; } = new();
        [Required]
        public List<RecepiesHaveTags> recepiesThatHave { get; set; } = new();
    }
}
