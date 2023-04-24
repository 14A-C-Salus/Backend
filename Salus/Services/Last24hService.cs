using Salus.Exceptions;

namespace Salus.Services.Last24hServices
{
    public class Last24hService : ILast24hService
    {
        private readonly DataContext _dataContext;
        private readonly GenericService<Recipe> _genericServicesRecipe;
        private readonly GenericService<Last24h> _genericServicesLast24hService;
        public Last24hService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _genericServicesRecipe = new(dataContext, httpContextAccessor);
            _genericServicesLast24hService = new(dataContext, httpContextAccessor);
        }

        public List<Last24h> GetAll()
        {
            UserProfile userProfile = _genericServicesRecipe.GetAuthenticatedUserProfile();
            return _genericServicesLast24hService.ReadAll().Where(l => l.userProfileId == userProfile.id).ToList();
        }

        public Last24h Add(AddRecipeToLast24H request)
        {
            var last24h = new Last24h();
            var recipe = _genericServicesRecipe.Read(request.recipeId);

            if (recipe == null)
                throw new ERecipeNotFound();
            else if (last24h.recipes == null)
                last24h.recipes = new List<Recipe>();

                last24h.recipes.Add(recipe);

            if (request.isLiquid)
                last24h.liquidInDl = request.dl;
            else
                last24h.liquidInDl = null;

            last24h.fat = recipe.fat * (request.portion / 100);
            last24h.kcal = recipe.kcal * (request.portion/100);
            last24h.protein = recipe.protein * (request.portion/100);
            last24h.carbohydrate = recipe.carbohydrate * (request.portion / 100);
            last24h.time = DateTime.Now;
            last24h.userProfile = _genericServicesLast24hService.GetAuthenticatedUserProfile();
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
