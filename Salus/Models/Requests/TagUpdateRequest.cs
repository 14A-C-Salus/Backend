

using static Salus.Models.Enums.RecipeEnums;

namespace Salus.Models.Requests
{
    public class TagUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public recipePropertiesEnum property { get; set; }
        public int maxValue { get; set; }
        public int minValue { get; set; }
    }
}
