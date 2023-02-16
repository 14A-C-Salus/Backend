using Salus.Controllers.Models.AuthModels;
using System.Xml.Linq;
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

        //public methods
        public void Delete(int recipeId)
        {
            var recipe = _genericServices.Read(recipeId);
            if (recipe == null)
                throw new Exception("This recipe doesn't exist.");
            if (recipe.Author != _genericServices.GetAuthenticatedUserProfile())
                throw new Exception("Only the author can delete the recipe.");
            _genericServices.Delete(recipe);
        }

        public Recipe WriteRecipe(WriteRecipeRequest request)
        {
            var userProfile = _genericServices.GetAuthenticatedUserProfile();
            var ingredients = GetAllIngredients(request.ingredientIds).Result;

            if (ingredients.Count() != request.ingredientPortionGramm.Count())
                throw new Exception("Some ingredient has no portion.");

            Check(request);

            var recipe = new Recipe()
            {
                Author = userProfile,
                name = request.name,
                method = request.method,
                timeInMinute = request.timeInMinutes,
                fat = 0,
                protein = 0,
                carbohydrate = 0,
                kcal = 0,
                gramm = 0,
                description = !request.generateDescription ? request.description : string.Empty
            };

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

                recipe.fat += (int)Math.Round((decimal)(
                    ((oil.calIn14Ml / 1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    / 9 //kcal to fat
                    * 0.018)); //1.8% of the oil is in the food

                recipe.kcal += (int)Math.Round((decimal)(
                    ((oil.calIn14Ml / 1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    * 0.018)); //1.8% of the oil is in the food

                recipe.oilPortionMl = request.oilPortionMl;
            }

            if (request.generateDescription)
                recipe.description = GenerateDescription(recipe);

            _genericServices.Create(recipe);

            return recipe;
        }

        public Recipe Update(UpdateRecipeRequest request)
        {

            var userProfile = _genericServices.GetAuthenticatedUserProfile();
            var ingredients = GetAllIngredients(request.ingredientIds).Result;

            if (ingredients.Count() != request.ingredientPortionGramm.Count())
                throw new Exception("Some ingredient has no portion.");

            UpdateCheck(request);
            var recipe = _genericServices.Read(request.recipeId);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (request.saveAs == false && recipe.Author != userProfile)
                throw new Exception("Only the author can modify the recipe, please check in the \"save as\" option, if you want to create a new recipe base on this one!");
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            recipe.Author = userProfile;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            recipe.name = request.name == string.Empty ? recipe.name : request.name;
            recipe.method = request.method == RecipeEnums.makeingMethodEnum.nondefined ? recipe.method : request.method;
            recipe.fat = 0;
            recipe.protein = 0;
            recipe.carbohydrate = 0;
            recipe.kcal = 0;
            recipe.gramm = 0;
            recipe.description = request.description == string.Empty ? recipe.description : request.description;
            
            //todo: ebből 2 van (ugyanaz, mint writerecipe nél)
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

                recipe.fat += (int)Math.Round((decimal)(
                    ((oil.calIn14Ml / 1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    / 9 //kcal to fat
                    * 0.018)); //1.8% of the oil is in the food

                recipe.kcal += (int)Math.Round((decimal)(
                    ((oil.calIn14Ml / 1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    * 0.018)); //1.8% of the oil is in the food

                recipe.oilPortionMl = request.oilPortionMl;
            }

            if (request.generateDescription)
                recipe.description = GenerateDescription(recipe);

            recipe = request.saveAs ? _genericServices.Create(recipe) : _genericServices.Update(recipe);

            return recipe;
        }

        //private methods
        private void Check(WriteRecipeRequest request)
        {
            if (request.method == makeingMethodEnum.nondefined) throw new Exception("Makeing method is required.");
            if (request.name.Length < 2 || request.name.Length > 200) throw new Exception("Invalid name.");
            if (request.timeInMinutes < 0 || request.timeInMinutes > 2000) throw new Exception("Invalid time.");
            if (request.oilPortionMl < 0 || request.oilPortionMl > 2000) throw new Exception("Invalid oil portion.");
            if (!request.generateDescription && (request.description.Length < 10 || request.description.Length > 2000)) throw new Exception("Invalid description.");
        }

        private void UpdateCheck(UpdateRecipeRequest request)
        {
            if ((request.name.Length < 2 && request.name != string.Empty) || request.name.Length > 200) throw new Exception("Invalid name.");
            if ((request.timeInMinutes < 0 && request.timeInMinutes != -1) || request.timeInMinutes > 2000) throw new Exception("Invalid time.");
            if ((request.oilPortionMl < 0 && request.oilPortionMl != -1) || request.oilPortionMl > 2000) throw new Exception("Invalid oil portion.");
            if (!request.generateDescription && ((request.description.Length < 10 && request.description != string.Empty) || request.description.Length > 2000)) throw new Exception("Invalid description.");
        }

        private string GenerateDescription(Recipe recipe)
        {
            string desc = string.Empty;
            desc += $"Name: {recipe.name} \n," +
                    $"Ingredients:\n";
#pragma warning disable CS8604 // Possible null reference argument.
            for (int i = 0; i < recipe.ingredients.Count(); i++)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var ingredient = recipe.ingredients[i].food.name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                var portion = recipe.ingredients[i].portionInGramm.ToString();
                desc += $"\t {ingredient} - {portion} g \n";
            }
#pragma warning restore CS8604 // Possible null reference argument.
            var methodVerb = string.Empty;
            switch (recipe.method)
            {
                case makeingMethodEnum.nondefined:
                    break;
                case makeingMethodEnum.baking:
                    methodVerb = "Bake";
                    break;
                case makeingMethodEnum.frying:
                    methodVerb = "Fry";
                    break;
                case makeingMethodEnum.roasting:
                    methodVerb = "Roast";
                    break;
                case makeingMethodEnum.cooking:
                    methodVerb = "Cook";
                    break;
                default:
                    break;
            }
            desc += $"{methodVerb} for {recipe.timeInMinute} minute.";
            return desc;
        }

        private async Task<List<Food>> GetAllIngredients(List<int>? ids)
        {
            if (ids == null || ids.Count() == 0)
                throw new Exception("There are no ingredients!");

            List<Food> ingredients = new();

            foreach (var ingredientId in ids)
            {
                ingredients.Add(await _dataContext.Set<Food>().SingleAsync(i => i.id == ingredientId));
            }

            return ingredients;
        }
    }
}
