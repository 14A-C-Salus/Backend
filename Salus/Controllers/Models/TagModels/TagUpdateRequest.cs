using static Salus.Controllers.Models.FoodModels.FoodEnums;

namespace Salus.Controllers.Models.TagModels
{
    public class TagUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public foodPropertiesEnum property { get; set; }
        public int maxValue { get; set; }
        public int minValue { get; set; }
    }
}
