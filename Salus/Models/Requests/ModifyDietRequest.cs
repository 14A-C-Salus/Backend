namespace Salus.Models.Requests
{
    public class ModifyDietRequest
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int? minFat { get; set; }
        public int? minCarbohydrate { get; set; }
        public int? minProtein { get; set; }
        public int? minKcal { get; set; }
        public int? minDl { get; set; }
        public int? maxFat { get; set; }
        public int? maxCarbohydrate { get; set; }
        public int? maxProtein { get; set; }
        public int? maxKcal { get; set; }
        public int? maxDl { get; set; }
    }
}