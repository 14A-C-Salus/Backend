namespace Salus.Controllers.Models.FoodModels
{
    public class FoodUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int? kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
    }
}
