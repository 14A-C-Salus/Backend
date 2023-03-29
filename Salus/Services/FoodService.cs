using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Salus.Controllers.Models.FoodModels;
using Salus.Exceptions;
using System.Xml.Linq;

namespace Salus.Services.FoodServices
{
    public class FoodService : IFoodService
    {
        private readonly DataContext _dataContext;
        private readonly GenericService<Food> _genericServicesFood;
        private readonly GenericService<Tag> _genericServicesTag;
        public FoodService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _genericServicesFood = new(dataContext, httpContextAccessor);
            _genericServicesTag = new(dataContext, httpContextAccessor);
        }

        public List<Tag> GetRecommendedTags(int foodId)
        {
            var food = _genericServicesFood.Read(foodId);
            if (food == null)
                throw new EFoodNotFound();
            List<Tag> tags = new();
            foreach (Tag tag in _genericServicesTag.ReadAll())
            {
                tag.food = food;
                if (tag.recommend)
                    tags.Add(tag);
            }
            return tags;
        }

        public Food AddTags(AddTagsToFoodRequest request)
        {
            var food = _genericServicesFood.Read(request.foodId);
            if (food == null)
                throw new EFoodNotFound();

            List<FoodsHaveTags> foodHasTags = new();
            foreach (var tagId in request.tagIds)
            {
                var tag = _genericServicesTag.Read(tagId);
                if (tag == null)
                    throw new ETagNotFound();

                var foodHasTag = new FoodsHaveTags
                {
                    food = food,
                    tag = tag
                };
                foodHasTags.Add(foodHasTag);

                if (_dataContext.Set<FoodsHaveTags>().Any(fHT => fHT.foodId == foodHasTag.foodId && fHT.tagId == foodHasTag.tagId))
                    throw new EFoodAlreadyHasTag();

                _dataContext.Set<FoodsHaveTags>().Add(foodHasTag);
                tag.foodsThatHave.Add(foodHasTag);
            }

            food.tags = foodHasTags;
            food = _genericServicesFood.Update(food);

            return food;
        }

        public Food Create(FoodCreateRequest request)
        {
            var food = new Food()
            {
                name = request.name,
                carbohydrate = request.carbohydrate,
                fat = request.fat,
                protein = request.protein,
                verifeid = false,
                kcal = 0
            };
            food.kcal = (int)(request.kcal == null ? CalculateKcal(food):request.kcal);
            CheckData(food);
            food = _genericServicesFood.Create(food);
            return food;
        }

        public void Delete(int id)
        {
            var food = _genericServicesFood.Read(id);
            if (food == null)
                throw new EFoodNotFound();
            _genericServicesFood.Delete(food);
        }

        public Food Update(FoodUpdateRequest request)
        {
            var food = _genericServicesFood.Read(request.id);
            if (food == null)
                throw new EFoodNotFound();

            food.name = request.name.Length == 0 ? food.name : request.name;
            food.carbohydrate = request.carbohydrate == -1 ? food.carbohydrate : request.carbohydrate;
            food.fat = request.fat == -1 ? food.fat : request.fat;
            food.protein = request.protein == -1 ? food.protein : request.protein;
            food.kcal = (int)(request.kcal == null ? CalculateKcal(food) : request.kcal);

            CheckData(food);
            food = _genericServicesFood.Update(food);
            return food;
        }

        public Food VerifyUnVerify(int id)
        {
            var food = _genericServicesFood.Read(id);
            if (food == null)
                throw new EFoodNotFound();
            food.verifeid = !food.verifeid;
            food = _genericServicesFood.Update(food);
            return food;
        }
        private int CalculateKcal(Food food)
        {
            return (int) Math.Round((food.carbohydrate*3.86)+(food.fat*9)+(food.protein*4));
        }
        private void CheckData(Food food)
        {
            if (food.name.Length > 50)
                throw new EFoodNameLength();
            if (food.name.Length < 5)
                throw new EFoodNameNull();
            if (food.fat > 100)
                throw new EFoodFatValue();
            if (food.protein > 100)
                throw new EFoodProteinValue();
            if (food.carbohydrate > 100)
                throw new EFoodCarbohydrateValue();
            if (food.fat < 0)
                throw new EFoodFatNegativeValue();
            if (food.protein < 0)
                throw new EFoodProteinNegativeValue();
            if (food.carbohydrate < 0)
                throw new EFoodCarbohydrateNegativeValue();
            if (food.kcal < 0)
                throw new EFoodCarbohydrateValue();
        }
    }
}
