namespace Salus.Controllers.Models.Last24hModels
{
    public class Last24h
    {
        public int id { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public int liquidInDl { get; set; }
        public int minLiquidInDl { get; set; }
        public int maxKcal { get; set; }
        public UserProfile userProfile { get; set; }
        public int userProfileId { get; set; }
        public List<Food> foods { get; set; }
    }
}
