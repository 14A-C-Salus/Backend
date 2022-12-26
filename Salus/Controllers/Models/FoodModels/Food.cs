namespace Salus.Controllers.Models.FoodModels
{
    public class Food
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public bool verifeid { get; set; } = false;

        //Connections
        public IList<RecipesIncludeIngredients> recipes { get; set; }
        public IList<FoodsHaveTags> tags { get; set; }
        public Last24h? last24h { get; set; }
    }
}
