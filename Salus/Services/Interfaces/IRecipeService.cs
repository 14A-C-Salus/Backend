namespace Salus.Services.RecipeServices
{
    public interface IRecipeService
    {
        Recipe WriteRecipe(WriteRecipeRequest request);
        Recipe Update(UpdateRecipeRequest request);
        public void Delete(int recipeId);
    }
}
