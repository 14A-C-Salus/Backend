using Salus.Controllers.Models.FoodModels;
using static Salus.Controllers.Models.RecipeModels.RecipeEnums;

namespace Salus.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly DataContext _dataContext;
        private readonly GenericService<Recipe> _genericServices;

        public RecipeService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _genericServices = new(dataContext, httpContextAccessor);
        }

        public Recipe WriteRecipe(WriteRecipeRequest request)
        {
            var userProfile = _genericServices.GetAuthenticatedUserProfile();
            var ingredients = GetAllIngredients(request).Result;

            if (ingredients.Count() != request.ingredientPortionGramm.Count())
                throw new Exception("Some ingredient has no portion.");

            var recipe = new Recipe()
            {
                Author = userProfile,
                name = request.name,
                fat = 0,
                protein = 0,
                carbohydrate = 0,
                kcal = 0
            };
            //TODO: Use generic sevice
            
            for (int i = 0; i < ingredients.Count; i++)
            {
                var ingredient = ingredients[i];
                var portion = request.ingredientPortionGramm[i];
                recipe.fat += ingredient.fat * portion;
                recipe.protein += ingredient.protein * portion;
                recipe.carbohydrate += ingredient.carbohydrate * portion;
                recipe.kcal += ingredient.kcal * portion;

                if (ingredient.tags.Count() != 0)
                    foreach (var tag in ingredient.tags)
                    {
                        if (tag.tag == null)
                            throw new Exception(); //TODO Idk mi ez
                        recipe.tags.Add(tag.tag);
                    }
            }
            //TODO: sütési metódussal számolni
            if (request.method == makeingMethodEnum.frying)
            {
                var oil = _dataContext.Set<Oil>().Find(request.oilId);
                if (oil == null)
                    throw new Exception("You need to choose an oil, if you fry something.");
                if (request.oilPortionMl <= 0 || request.oilPortionMl == null)
                    throw new Exception("Oil has no portion!");

                recipe.fat += (int) Math.Round((decimal)(oil.calIn14Ml/1000 * request.oilPortionMl / 4.5));
            }
            //TODO: Check

            _genericServices.Create(recipe);

            return recipe;
        }

        public async Task<List<Food>> GetAllIngredients(WriteRecipeRequest request)
        {
            if (request.ingredientIds == null || request.ingredientIds.Count() == 0)
                throw new Exception("There are no ingredients!");

            List<Food> ingredients = new();

            foreach (var ingredientId in request.ingredientIds)
            {
                ingredients.Add(await _dataContext.Set<Food>().SingleAsync(i => i.id == ingredientId));
            }

            return ingredients;
        }
    }
}
