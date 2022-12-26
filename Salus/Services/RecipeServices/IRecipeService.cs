namespace Salus.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<Recipe> WriteRecipe(WriteRecipeRequest request);
    }
}
