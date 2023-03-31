﻿using Salus.Controllers.Models.AuthModels;
using Salus.Exceptions;
using System.Xml.Linq;
using static Salus.Controllers.Models.RecipeModels.RecipeEnums;

namespace Salus.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly DataContext _dataContext;
        private readonly GenericService<Recipe> _genericServices;
        private readonly GenericService<Tag> _genericServicesTag;

        public RecipeService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _genericServices = new(dataContext, httpContextAccessor);
            _genericServicesTag = new(dataContext, httpContextAccessor);
        }

        //----------------------------------------------------------------------------
        //------------------------------- Public Methods -----------------------------
        //----------------------------------------------------------------------------

        //--------- Tags Start ----------//
        public List<Tag> GetRecommendedTags(int recipeId)
        {
            var recipe = _genericServices.Read(recipeId);
            if (recipe == null)
                throw new EFoodNotFound();
            List<Tag> tags = new();
            foreach (Tag tag in _genericServicesTag.ReadAll())
            {
                tag.recipe = recipe;
                if (tag.recommend)
                    tags.Add(tag);
            }
            return tags;
        }

        public Recipe AddTags(AddTagsToFoodRequest request)
        {
            var recipe = _genericServices.Read(request.recipeId);
            if (recipe == null)
                throw new EFoodNotFound();

            List<RecepiesHaveTags> foodHasTags = new();
            foreach (var tagId in request.tagIds)
            {
                var tag = _genericServicesTag.Read(tagId);
                if (tag == null)
                    throw new ETagNotFound();

                var foodHasTag = new RecepiesHaveTags
                {
                    recipe = recipe,
                    tag = tag
                };
                foodHasTags.Add(foodHasTag);

                if (_dataContext.Set<RecepiesHaveTags>().Any(fHT => fHT.recipeId == foodHasTag.recipeId && fHT.tagId == foodHasTag.tagId))
                    throw new EFoodAlreadyHasTag();

                _dataContext.Set<RecepiesHaveTags>().Add(foodHasTag);
                tag.recepiesThatHave.Add(foodHasTag);
            }

            recipe.tags = foodHasTags;
            recipe = _genericServices.Update(recipe);

            return recipe;
        }
        //--------- Tags End ----------//

        //--------- Simple CRUD Start ----------//
        public Recipe CreateSimple(FoodCreateRequest request)
        {
            var recipe = new Recipe()
            {
                name = request.name,
                carbohydrate = request.carbohydrate,
                fat = request.fat,
                protein = request.protein,
                verifeid = false,
                kcal = 0
            };
            recipe.kcal = (int)(request.kcal == null ? CalculateKcal(recipe) : request.kcal);
            CheckData(recipe);
            recipe = _genericServices.Create(recipe);
            return recipe;
        }

        public Recipe UpdateSimple(FoodUpdateRequest request)
        {
            var recipe = _genericServices.Read(request.id);
            if (recipe == null)
                throw new EFoodNotFound();

            recipe.name = request.name.Length == 0 ? recipe.name : request.name;
            recipe.carbohydrate = request.carbohydrate == -1 ? recipe.carbohydrate : request.carbohydrate;
            recipe.fat = request.fat == -1 ? recipe.fat : request.fat;
            recipe.protein = request.protein == -1 ? recipe.protein : request.protein;
            recipe.kcal = (int)(request.kcal == null ? CalculateKcal(recipe) : request.kcal);

            CheckData(recipe);
            recipe = _genericServices.Update(recipe);
            return recipe;
        }
        //--------- Simple CRUD End ----------//


        //--------- CRUD Start ----------//
        public List<Recipe> GetAll(int authId) 
        {
            return _genericServices.ReadAll().Where(r => r.Author.id == authId).ToList();
        }

        public void Delete(int recipeId)
        {
            var recipe = _genericServices.Read(recipeId);
            if (recipe == null)
                throw new ERecipeNotFound();
            if (recipe.Author != _genericServices.GetAuthenticatedUserProfile())
                throw new EUnauthorized();
            _genericServices.Delete(recipe);
        }


        public Recipe Create(WriteRecipeRequest request)
        {
            var userProfile = _genericServices.GetAuthenticatedUserProfile();
            var ingredients = GetAllIngredients(request.ingredientIds).Result;

            if (ingredients.Count() != request.ingredientPortionGramm.Count())
                throw new EIngredientHasNoPortion();

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
                            recipe.tags.Add(foodsHasTag);

                var recipeIncludeIngredients = new RecipesIncludeIngredients()
                {
                    ingredient = ingredient,
                    recipe = recipe,
                    portionInGramm = portion
                };
                _dataContext.Set<RecipesIncludeIngredients>().Add(recipeIncludeIngredients);
            }

            if (request.method == makeingMethodEnum.frying)
            {
                var oil = _dataContext.Set<Oil>().Find(request.oilId);
                if (oil == null)
                    throw new EToFryNeedOil();
                if (request.oilPortionMl <= 0 || request.oilPortionMl == null)
                    throw new EInvalidOilPortion();

                recipe.fat += (int)Math.Round((decimal)(
                    ((oil.calIn14Ml / 1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    / 9 //kcal to fat
                    * 0.018)); //1.8% of the oil is in the recipe

                recipe.kcal += (int)Math.Round((decimal)(
                    ((oil.calIn14Ml / 1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    * 0.018)); //1.8% of the oil is in the recipe

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
            var recipe = _genericServices.Read(request.recipeId);

            if (recipe == null)
                throw new ERecipeNotFound();

            var ingredients = GetAllIngredients(request.ingredientIds).Result;

            if (ingredients.Count() != request.ingredientPortionGramm.Count())
                throw new EIngredientHasNoPortion();

            UpdateCheck(request);

            if (request.saveAs == false && recipe.Author != userProfile)
                throw new EOnlyAuthorCanModifyRecipe();
            recipe.Author = userProfile;
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
                            recipe.tags.Add(foodsHasTag);

                var recipeIncludeIngredients = new RecipesIncludeIngredients()
                {
                    ingredient = ingredient,
                    recipe = recipe,
                    portionInGramm = portion
                };
                _dataContext.Set<RecipesIncludeIngredients>().Add(recipeIncludeIngredients);
            }

            if (request.method == makeingMethodEnum.frying)
            {
                var oil = _dataContext.Set<Oil>().Find(request.oilId);
                if (oil == null)
                    throw new EToFryNeedOil();
                if (request.oilPortionMl <= 0 || request.oilPortionMl == null)
                    throw new EInvalidOilPortion();

                recipe.fat += (int)Math.Round((decimal)(
                    ((oil.calIn14Ml / 1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    / 9 //kcal to fat
                    * 0.018)); //1.8% of the oil is in the recipe

                recipe.kcal += (int)Math.Round((decimal)(
                    ((oil.calIn14Ml / 1000) //cal to kcal
                    / 14) //14Ml to 1Ml
                    * request.oilPortionMl //1Ml to request Ml
                    * 0.018)); //1.8% of the oil is in the recipe

                recipe.oilPortionMl = request.oilPortionMl;
            }

            if (request.generateDescription)
                recipe.description = GenerateDescription(recipe);

            recipe = request.saveAs ? _genericServices.Create(recipe) : _genericServices.Update(recipe);

            return recipe;
        }
        //--------- Simple CRUD End ----------//

        //--------- Other Start ----------//
        public Recipe VerifyUnVerify(int id)
        {
            var recipe = _genericServices.Read(id);
            if (recipe == null)
                throw new EFoodNotFound();
            recipe.verifeid = !recipe.verifeid;
            recipe = _genericServices.Update(recipe);
            return recipe;
        }
        //--------- Other End ----------//


        //----------------------------------------------------------------------------
        //------------------------------- Private Methods -----------------------------
        //----------------------------------------------------------------------------

        private string GenerateDescription(Recipe recipe)
        {
            string desc = string.Empty;
            desc += $"Name: {recipe.name} \n," +
                    $"Ingredients:\n";
            //Reason: We checked before in "GetAllIngredients".
#pragma warning disable CS8604 // Possible null reference argument. 
            for (int i = 0; i < recipe.ingredients.Count(); i++)
            {
                //Reason: Nullable need to the optional relationship, but it can't be null here.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var ingredient = recipe.ingredients[i].recipe.name;
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

        private async Task<List<Recipe>> GetAllIngredients(List<int>? ids)
        {
            if (ids == null || ids.Count() == 0)
                throw new EIngredientsMissing();

            List<Recipe> ingredients = new();

            foreach (var ingredientId in ids)
            {
                ingredients.Add(await _dataContext.Set<Recipe>().SingleAsync(i => i.id == ingredientId));
            }

            return ingredients;
        }

        private int CalculateKcal(Recipe recipe)
        {
            return (int)Math.Round((recipe.carbohydrate * 3.86) + (recipe.fat * 9) + (recipe.protein * 4));
        }


        //--------- Checks Start ----------//
        private void Check(WriteRecipeRequest request)
        {
            if (request.method == makeingMethodEnum.nondefined) throw new EMakingMethodMissing();
            if (request.name.Length < 2 || request.name.Length > 200) throw new ERecipeName();
            if (request.timeInMinutes < 0 || request.timeInMinutes > 2000) throw new EInvalidTime();
            if (request.oilPortionMl < 0 || request.oilPortionMl > 2000) throw new EInvalidOilPortion();
            if (!request.generateDescription && (request.description.Length < 10 || request.description.Length > 2000)) throw new EInvalidDescription();
        }

        private void UpdateCheck(UpdateRecipeRequest request)
        {
            if ((request.name.Length < 2 && request.name != string.Empty) || request.name.Length > 200) throw new ERecipeName();
            if ((request.timeInMinutes < 0 && request.timeInMinutes != -1) || request.timeInMinutes > 2000) throw new EInvalidTime();
            if ((request.oilPortionMl < 0 && request.oilPortionMl != -1) || request.oilPortionMl > 2000) throw new EInvalidOilPortion();
            if (!request.generateDescription && ((request.description.Length < 10 && request.description != string.Empty) || request.description.Length > 2000)) throw new EInvalidDescription();
        }

        private void CheckData(Recipe recipe)
        {
            if (recipe.name.Length > 50)
                throw new EFoodNameLength();
            if (recipe.name.Length < 5)
                throw new EFoodNameNull();
            if (recipe.fat > 100)
                throw new EFoodFatValue();
            if (recipe.protein > 100)
                throw new EFoodProteinValue();
            if (recipe.carbohydrate > 100)
                throw new EFoodCarbohydrateValue();
            if (recipe.fat < 0)
                throw new EFoodFatNegativeValue();
            if (recipe.protein < 0)
                throw new EFoodProteinNegativeValue();
            if (recipe.carbohydrate < 0)
                throw new EFoodCarbohydrateNegativeValue();
            if (recipe.kcal < 0)
                throw new EFoodCarbohydrateValue();
        }
        //--------- Checks End ----------//
    }
}
