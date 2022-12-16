namespace Salus.Controllers.Models.JoiningEntity
{
    public class RecipesIncludeIngredients
    {
        public int id { get; set; }
        public int portionInGramm { get; set; }
        public int recipeId { get; set; }
        public Recipe recipe { get; set; }
        public int foodId { get; set; }
        public Food food { get; set; }
    }
}
