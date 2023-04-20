namespace Salus.Models.Requests
{
    public class AddTagsToRecipeRequest
    {
        public int recipeId { get; set; }
        public List<int> tagIds { get; set; } = new();
    }
}
