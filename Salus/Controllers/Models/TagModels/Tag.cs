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
        public Food? food { get; set; }
        public foodPropertiesEnum? foodProperty { get; set; }
        public int? min { get; set; }
        public int? max { get; set; }
        [NotMapped]
        public bool recommend { 
            get 
            {
                if (food == null)
                    return false;
                switch (foodProperty)
                {
                    case foodPropertiesEnum.protein :
                        return food.protein > min && food.protein < max;
                    case foodPropertiesEnum.fat:
                        return food.fat > min && food.fat < max;
                    case foodPropertiesEnum.carbohydrate:
                        return food.carbohydrate > min && food.carbohydrate < max;
                    case foodPropertiesEnum.kcal:
                        return food.kcal > min && food.kcal < max;
                    default:
                        return false;
                }
            }
        }

        //Connection
        [JsonIgnore, Required]
        public List<UsersPreferTags> usersWhoPrefer { get; set; } = new();
        [Required]
        public List<FoodsHaveTags> foodsThatHave { get; set; } = new();
    }
}
