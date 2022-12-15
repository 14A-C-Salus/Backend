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

            if (request.ingredients == null)
                throw new Exception("There are no ingredients!");

            foreach (var ingredient in request.ingredients)
            {
                recipe.fat += ingredient.fat;
                recipe.protein += ingredient.protein;
                recipe.carbohydrate += ingredient.carbohydrate;
                recipe.kcal += ingredient.kcal;

                if (ingredient.tags.Count() != 0)
                {
                    foreach (var tag in ingredient.tags)
                    {
                        recipe.tags.Add(tag);
                    }
                }
            }

            //TODO: sütési metódussal számolni

            return recipe;
        }
    }
}
