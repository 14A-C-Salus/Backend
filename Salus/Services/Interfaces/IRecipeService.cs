namespace Salus.Services.RecipeServices
{
    public interface IRecipeService
    {
        List<Recipe> GetAll(int authId);
        Recipe Create(WriteRecipeRequest request);
        Recipe Update(UpdateRecipeRequest request);
        void Delete(int recipeId);
        Recipe CreateSimple(FoodCreateRequest request);
        Recipe UpdateSimple(FoodUpdateRequest request);
        Recipe VerifyUnVerify(int id);
        Recipe AddTags(AddTagsToFoodRequest request);
        List<Tag> GetRecommendedTags(int recipeId);
    }
}
