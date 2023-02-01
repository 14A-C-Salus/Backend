using Newtonsoft.Json;

namespace Salus.Controllers.Models.FoodModels
{
    public class Food
    {
        public int id { get; set; }
        public int kcal { get; set; }
        //Everything is /100 g
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public string name { get; set; } = string.Empty;
        public bool verifeid { get; set; } = false;

        //Connections
        [Required, JsonIgnore]
        public IList<RecipesIncludeIngredients>? recipes { get; set; }
        [Required, JsonIgnore]
        public IList<FoodsHaveTags>? tags { get; set; }
        public Last24h? last24h { get; set; }
    }
}
