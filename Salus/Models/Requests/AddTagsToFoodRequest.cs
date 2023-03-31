namespace Salus.Controllers.Models.FoodModels
{
    public class AddTagsToFoodRequest
    {
        public int recipeId { get; set; }
        public List<int> tagIds { get; set; } = new();
    }
}
