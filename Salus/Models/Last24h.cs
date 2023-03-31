using Newtonsoft.Json;

namespace Salus.Controllers.Models.Last24hModels
{
    public class Last24h
    {
        public int id { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public int? liquidInDl { get; set; }
        public DateTime time { get; set; }
        [Required, JsonIgnore]
        public UserProfile? userProfile { get; set; }
        public int userProfileId { get; set; }
        [JsonIgnore]
        public IList<Recipe>? recipes { get; set; }
    }
}
