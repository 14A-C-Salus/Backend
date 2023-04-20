

using static Salus.Models.Enums.RecipeEnums;

namespace Salus.Models.Requests
{
    public class UpdateRecipeRequest
    {
        [Required]
        public int recipeId { get; set; } = -1;
        public List<int>? ingredientIds { get; set; } = new();
        public List<int> ingredientPortionGramm { get; set; } = new();
        public makeingMethodEnum method { get; set; } = makeingMethodEnum.nondefined;
        public int? oilId { get; set; } = -1;
        public int? oilPortionMl { get; set; } = -1;
        public int timeInMinutes { get; set; } = -1;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool generateDescription { get; set; } = false;
        public bool saveAs { get; set; } = false;
    }
}
