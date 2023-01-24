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
                kcal = 0,
                gramm = 0,
                description = !request.generateDescription ? request.description : string.Empty
            };
            //TODO: Use generic sevice
            
            for (int i = 0; i < ingredients.Count; i++)
            {
                var ingredient = ingredients[i];
                var portion = request.ingredientPortionGramm[i];
                recipe.gramm += portion;
                recipe.fat += ingredient.fat * portion / 100;
                recipe.protein += ingredient.protein * portion / 100;
                recipe.carbohydrate += ingredient.carbohydrate * portion / 100;
                recipe.kcal += ingredient.kcal * portion / 100;

                if (ingredient.tags != null && ingredient.tags.Count() != 0)
                    foreach (var foodsHasTag in ingredient.tags)
                        if (foodsHasTag.tag != null)
                            recipe.tags.Add(foodsHasTag.tag);

                var recipeIncludeIngredients = new RecipesIncludeIngredients()
                {
                    food = ingredient,
                    recipe = recipe,
                    portionInGramm = portion
                };
                _dataContext.Set<RecipesIncludeIngredients>().Add(recipeIncludeIngredients);
            }

            if (request.method == makeingMethodEnum.frying)
            {
                var oil = _dataContext.Set<Oil>().Find(request.oilId);
                if (oil == null)
                    throw new Exception("You need to choose an oil, if you fry something.");
                if (request.oilPortionMl <= 0 || request.oilPortionMl == null)
                    throw new Exception("Oil has no portion!");

                recipe.fat += (int) Math.Round((decimal)(
                    ((oil.calIn14Ml/1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    / 9 //kcal to fat
                    / 10)); //10% of the oil is in the food

                recipe.kcal += (int)Math.Round((decimal)(
                    ((oil.calIn14Ml / 1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    / 10)); //10% of the oil is in the food

                recipe.oilPortionMl = request.oilPortionMl;
            }

            if (request.generateDescription)
                recipe.description = GenerateDescription(recipe);

            _genericServices.Create(recipe);

            return recipe;
        }

        private string GenerateDescription(Recipe recipe)
        {
            return $"Name: {recipe.name} \n," +
                $"";
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
