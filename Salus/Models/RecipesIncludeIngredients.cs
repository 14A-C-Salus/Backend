using Newtonsoft.Json;

namespace Salus.Models
{
    public class RecipesIncludeIngredients
    {
        public int id { get; set; }
        public int portionInGramm { get; set; }
        public int recipeId { get; set; }
        [Required, JsonIgnore]
        public Recipe? recipe { get; set; }
        public int ingredientId { get; set; }
        [Required, JsonIgnore]
        public Recipe? ingredient { get; set; }
    }
}
