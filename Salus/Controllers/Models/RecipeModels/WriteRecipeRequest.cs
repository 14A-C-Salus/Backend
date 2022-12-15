using static Salus.Controllers.Models.RecipeModels.RecipeEnums;

namespace Salus.Controllers.Models.RecipeModels
{
    public class WriteRecipeRequest
    {
        [Required]
        public List<Food>? ingredients { get; set; }
        public makeingMethodEnum method { get; set; } = makeingMethodEnum.nondefined;
        public int timeInMinutes { get; set; }
        public string description { get; set; } = string.Empty;
    }
}
