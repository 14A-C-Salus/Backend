namespace Salus.Controllers.Models.FoodModels
{
    public class Oil
    {
        public int id { get; set; }
        public int calIn14Ml { get; set; }
        public string name { get; set; } = string.Empty;
        public virtual List<Recipe>? recipes { get; set; }
    }
}
