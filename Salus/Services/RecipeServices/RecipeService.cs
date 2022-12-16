namespace Salus.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;
        public RecipeService(DataContext dataContext, IAuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        public async Task<Recipe> WriteRecipe(WriteRecipeRequest request)
        {
            var auth = await _dataContext.auths.FirstAsync(a => a.email == _authService.GetEmail());

            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);
            if (userProfile == null)
                throw new Exception("You need to create a user profile first!");

            var recipe = new Recipe()
            {
                Author = userProfile,
                name = request.name,
            };

            var ingredients = GetAllIngredients(request).Result;

            foreach (var ingredient in ingredients)
            {
                recipe.fat += ingredient.fat;
                recipe.protein += ingredient.protein;
                recipe.carbohydrate += ingredient.carbohydrate;
                recipe.kcal += ingredient.kcal;

                if (ingredient.tagIds.Count() != 0)
                {
                    foreach (var tag in ingredient.tagIds)
                    {
                        recipe.tagIds.Add(tag);
                    }
                }
            }

            _dataContext.recipes.Add(recipe);
            await _dataContext.SaveChangesAsync();
            //TODO: sütési metódussal számolni

            return recipe;
        }

        public async Task<List<Food>> GetAllIngredients(WriteRecipeRequest request)
        {
            if (request.ingredientIds == null || request.ingredientIds.Count() == 0)
                throw new Exception("There are no ingredients!");

            List<Food> ingredients = new();

            foreach (var ingredientId in request.ingredientIds)
            {
                ingredients.Add(await _dataContext.foods.SingleAsync(i => i.id == ingredientId));
            }

            return ingredients;
        }
    }
}
