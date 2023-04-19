namespace Salus.Services.RecipeServices
{
    public interface IRecipeService
    {
        List<Recipe> GetAll(int authId);
        Recipe Create(WriteRecipeRequest request);
        Recipe Update(UpdateRecipeRequest request);
        void Delete(int recipeId);
        Recipe CreateSimple(RecipeCreateRequest request);
        Recipe UpdateSimple(RecipeUpdateRequest request);
        Recipe VerifyUnVerify(int id);
        Recipe AddTags(AddTagsToRecipeRequest request);
        List<Tag> GetRecommendedTags(int recipeId);
        List<UsersLikeRecipes> LikeUnlike(int recipeId);
        List<Recipe> GetAllByTagId(int tagId);
    }
}
