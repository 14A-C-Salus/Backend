namespace Salus.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly DataContext _dataContext;
        private readonly IGenericServices<Recipe> _crud;

        public RecipeService(DataContext dataContext, IGenericServices<Recipe> crud)
        {
            _dataContext = dataContext;
            _crud = crud;
        }

        public Recipe WriteRecipe(WriteRecipeRequest request)
        {
            var userProfile = _crud.GetAuthenticatedUserProfile(_dataContext).Result;

            var recipe = new Recipe()
            {
                Author = userProfile,
                name = request.name,
                fat = 0,
                protein = 0,
                carbohydrate = 0,
                kcal = 0
            };

            var ingredients = GetAllIngredients(request).Result;

            foreach (var ingredient in ingredients)
            {
                recipe.fat += ingredient.fat;
                recipe.protein += ingredient.protein;
                recipe.carbohydrate += ingredient.carbohydrate;
                recipe.kcal += ingredient.kcal;

                if (ingredient.tags.Count() != 0)
                {
                    foreach (var tag in ingredient.tags)
                    {
                        recipe.tags.Add(tag.tag);
                    }
                }
            }

            _crud.Create(recipe);
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
