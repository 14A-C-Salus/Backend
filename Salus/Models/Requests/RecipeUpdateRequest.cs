namespace Salus.Models.Requests
{
    public class RecipeUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int? kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
    }
}
