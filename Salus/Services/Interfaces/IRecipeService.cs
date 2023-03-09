namespace Salus.Services.RecipeServices
{
    public interface IRecipeService
    {
        public List<Recipe> GetAll(int authId);
        Recipe Create(WriteRecipeRequest request);
        Recipe Update(UpdateRecipeRequest request);
        public void Delete(int recipeId);
    }
}
