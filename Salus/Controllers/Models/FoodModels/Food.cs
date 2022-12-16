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
        public List<int> tagIds { get; set; } = new ();

        //Connections
        public List<RecipesIncludeIngredients> recipes { get; set; }
        public List<FoodsHaveTags> tags { get; set; }
        public Last24h last24h { get; set; }
    }
}
