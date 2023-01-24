namespace Salus.Services.RecipeServices
{
    public interface IRecipeService
    {
        Recipe WriteRecipe(WriteRecipeRequest request);
    }
}
