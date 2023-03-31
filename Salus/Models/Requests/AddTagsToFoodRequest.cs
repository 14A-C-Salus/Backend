namespace Salus.Controllers.Models.RecipeModels
{
    public class AddTagsToRecipeRequest
    {
        public int recipeId { get; set; }
        public List<int> tagIds { get; set; } = new();
    }
}
