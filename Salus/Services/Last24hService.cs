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
    }
}
