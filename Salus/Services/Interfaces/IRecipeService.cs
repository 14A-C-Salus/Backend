namespace Salus.Services.Interfaces
{
    public interface IRecipeService
    {
        void Delete(int recipeId);
        Recipe Create(WriteRecipeRequest request);
        Recipe Update(UpdateRecipeRequest request);
        Recipe CreateSimple(RecipeCreateRequest request);
        Recipe UpdateSimple(RecipeUpdateRequest request);
        Recipe VerifyUnVerify(int id);
        Recipe AddTags(AddTagsToRecipeRequest request);
        List<Tag> GetRecommendedTags(int recipeId);
        List<UsersLikeRecipes> LikeUnlike(int recipeId);
        List<Recipe> GetAllByAuthId(int authId);
        List<Recipe> GetAll();
        List<Recipe> GetAllByTagId(int tagId);
        List<Recipe> GetRecipesByName(string name);
    }
}
