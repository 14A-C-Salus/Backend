using System.Diagnostics.CodeAnalysis;

namespace Salus.Controllers.Models.RecipeModels
{
    public class RecipeCreateRequest
    {
        public string name { get; set; } = string.Empty;
        public int? kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
    }
}
