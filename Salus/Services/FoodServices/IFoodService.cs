namespace Salus.Services.FoodServices
{
    public interface IFoodService
    {
        public Food Create(FoodCreateRequest request);
        public Food Update(FoodUpdateRequest request);
        public Food VerifyUnVerify(int id);
        public void Delete(int id);
        public Food AddTags(AddTagsToFoodRequest request);
    }
}
