namespace Salus.Models.Requests
{
    public class CreateDietRequest
    {
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public Between<int>? fat { get; set; }
        public Between<int>? carbohydrate { get; set; }
        public Between<int>? protein { get; set; }
        public Between<int>? kcal { get; set; }
        public Between<int>? dl { get; set; }
    }
}