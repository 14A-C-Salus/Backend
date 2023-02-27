namespace Salus.Services.RecipeServices
{
    public interface IRecipeService
    {
        Recipe Create(WriteRecipeRequest request);
        Recipe Update(UpdateRecipeRequest request);
        public void Delete(int recipeId);
    }
}
