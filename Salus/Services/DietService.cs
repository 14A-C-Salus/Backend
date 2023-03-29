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
                carbohydrate = request.carbohydrate,
                description = request.description,
                dl = request.dl,
                fat = request.fat,
                kcal = request.kcal,
                name = request.name,
                protein = request.protein
            };
            CheckDiet(diet);
            _genericServices.Create(diet);
            return diet;
        }

        private void CheckDiet(Diet diet)
        {
            if (diet.carbohydrate != null && (diet.carbohydrate.Maximum > 10000 || diet.carbohydrate.Minimum < 0))
                throw new EInvalidCarbohydrate();
            if (diet.fat != null && (diet.fat.Maximum > 10000 || diet.fat.Minimum < 0))
                throw new EInvalidFat();
            if (diet.dl != null && (diet.dl.Maximum > 500 || diet.dl.Minimum < 0))
                throw new EInvalidDl();
            if (diet.kcal != null && (diet.kcal.Maximum > 10000 || diet.kcal.Minimum < 0))
                throw new EInvalidKcal();
            if (diet.name.Length < 3 || diet.name.Length > 255)
                throw new EInvalidName();
            if (diet.protein != null && (diet.protein.Maximum > 10000 || diet.protein.Minimum < 0))
                throw new EInvalidProtein();
            if (diet.description.Length < 5 || diet.description.Length > 1000)
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

            diet.carbohydrate = request.carbohydrate == null ? diet.carbohydrate: request.carbohydrate;
            diet.dl = request.dl == null ? diet.dl : request.dl;
            diet.fat = request.fat == null ? diet.fat : request.fat;
            diet.kcal = request.kcal == null ? diet.kcal : request.kcal;
            diet.name = request.name == null ? diet.name : request.name;
            diet.protein = request.protein == null ? diet.protein : request.protein;
            CheckDiet(diet);
            diet = _genericServices.Update(diet);
            return diet;
        }
    }
}
