namespace Salus.Controllers.Models.FoodModels
{
    public class AddTagsToFoodRequest
    {
        public int foodId { get; set; }
        public List<int> tagIds { get; set; }
    }
}
