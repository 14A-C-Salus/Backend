using Salus.Exceptions;
using Salus.Models;
using Salus.Models.Requests;

namespace Salus.Services
{
    public class DietService : IDietService
    {
        private readonly GenericService<Diet> _genericServices;
        public DietService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _genericServices = new(dataContext, httpContextAccessor);
        }

        public Diet? Create(CreateDietRequest request)
        {
            var diet = new Diet()
            {
                description = request.description,
                name = request.name,
                minDl = request.minDl,
                minFat = request.minFat,
                minKcal = request.minKcal,
                minCarbohydrate = request.minCarbohydrate,
                minProtein = request.minProtein,
                maxDl = request.maxDl,
                maxFat = request.maxFat,
                maxKcal = request.maxKcal,
                maxCarbohydrate = request.maxCarbohydrate,
                maxProtein = request.maxProtein
            };
            CheckDiet(diet);
            _genericServices.Create(diet);
            return diet;
        }

        private void CheckDiet(Diet diet)
        {
            if (diet.maxCarbohydrate != null && (diet.maxCarbohydrate > 10000 || diet.maxCarbohydrate < 0))
                throw new EInvalidCarbohydrate();
            if (diet.maxFat != null && (diet.maxFat > 10000 || diet.maxFat < 0))
                throw new EInvalidFat();
            if (diet.maxDl != null && (diet.maxDl > 500 || diet.maxDl < 0))
                throw new EInvalidDl();
            if (diet.maxKcal != null && (diet.maxKcal > 10000 || diet.maxKcal < 0))
                throw new EInvalidKcal();
            if (string.IsNullOrEmpty(diet.name) || diet.name.Length < 3 || diet.name.Length > 255)
                throw new EInvalidName();
            if (diet.maxProtein != null && (diet.maxProtein > 10000 || diet.maxProtein < 0))
                throw new EInvalidProtein();
            if (string.IsNullOrEmpty(diet.description) || diet.description.Length < 5 || diet.description.Length > 1000)
                throw new EInvalidDescription();
        }


        public void Delete(int id)
        {
            var diet = _genericServices.Read(id);
            if (diet == null)
                throw new EDietNotFound();
            _genericServices.Delete(diet);
        }

        public Diet? Modify(ModifyDietRequest request)
        {
            var diet = _genericServices.Read(request.id);
            if (diet == null)
                throw new EDietNotFound();

            diet.maxCarbohydrate = request.maxCarbohydrate ?? diet.maxCarbohydrate;
            diet.maxDl = request.maxDl ?? diet.maxDl;
            diet.maxFat = request.maxFat ?? diet.maxFat;
            diet.maxKcal = request.maxKcal ?? diet.maxKcal;
            diet.name = string.IsNullOrEmpty(request.name) ? diet.name : request.name;
            diet.maxProtein = request.maxProtein ?? diet.maxProtein;

            CheckDiet(diet);

            diet = _genericServices.Update(diet);

            return diet;
        }
    }
}
