using Salus.Controllers.Models.FoodModels;
using Salus.Exceptions;
using Salus.Models.Requests;

namespace Salus.Services.Last24hServices
{
    public class Last24hService : ILast24hService
    {
        private readonly DataContext _dataContext;
        private readonly GenericService<Food> _genericServicesFood;
        private readonly GenericService<Last24h> _genericServicesLast24hService;
        private readonly GenericService<UserProfile> _genericServicesUserProfile;
        public Last24hService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _genericServicesFood = new(dataContext, httpContextAccessor);
            _genericServicesLast24hService = new(dataContext, httpContextAccessor);
            _genericServicesUserProfile = new(dataContext, httpContextAccessor);
        }

        public Last24h Add(AddFoodToLast24H request)
        {
            var last24h = new Last24h();
            var food = _genericServicesFood.Read(request.foodId);

            if (food == null)
                throw new EFoodNotFound();
            else if (last24h.foods == null)
                last24h.foods = new List<Food>();

                last24h.foods.Add(food);

            if (request.isLiquid)
                last24h.liquidInDl = request.dl;
            else
                last24h.liquidInDl = null;

            last24h.fat = food.fat * (request.portion / 100);
            last24h.kcal = food.kcal * (request.portion/100);
            last24h.protein = food.protein * (request.portion/100);
            last24h.carbohydrate = food.carbohydrate * (request.portion / 100);

            return _genericServicesLast24hService.Create(last24h);
        }

        public void Delete(int id)
        {
            var last24H = _genericServicesLast24hService.Read(id);
            if (last24H == null)
                throw new ELast24HNotFound();
            _genericServicesLast24hService.Delete(last24H);
        }

        public Last24h DoublePortion(int id)
        {
            return SetPortion(id, 2);
        }

        private Last24h SetPortion(int id, double multiplier)
        {
            var last24H = _genericServicesLast24hService.Read(id);
            if (last24H == null)
                throw new ELast24HNotFound();
            last24H.fat = (int)Math.Floor(last24H.fat * multiplier);
            last24H.kcal *= (int)Math.Floor(last24H.kcal * multiplier);
            last24H.protein *= (int)Math.Floor(last24H.protein * multiplier);
            last24H.carbohydrate *= (int)Math.Floor(last24H.carbohydrate * multiplier);
            return last24H;
        }

        public Last24h HalfPortion(int id)
        {
            return SetPortion(id, 1/2);
        }

        public Last24h QuarterPortion(int id)
        {
            return SetPortion(id, 1/4);
        }

        public Last24h ThirdPortion(int id)
        {
            return SetPortion(id, 1/3);
        }
    }
}
