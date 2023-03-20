using Salus.Controllers.Models.FoodModels;
using Salus.Exceptions;
using System.Security.Cryptography;

namespace Salus.Services.FoodServices
{
    public class OilService:IOilService
    {
        private readonly GenericService<Oil> _genericServices;
        public OilService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _genericServices = new(dataContext, httpContextAccessor);
        }

        public Oil Create(OilCreateRequest request)
        {
            var oil = new Oil()
            {
                name = request.name,
                calIn14Ml = request.calIn14Ml,
            };
            CheckData(oil);
            oil = _genericServices.Create(oil);
            return oil;
        }

        public Oil Update(OilUpdateRequest request)
        {
            var oil = _genericServices.Read(request.id);
            if (oil == null)
                throw new EOilNotFound();

            oil.name = request.name.Length == 0 ? oil.name : request.name;
            oil.calIn14Ml = request.calIn14Ml == 0 ? oil.calIn14Ml : request.calIn14Ml;
            CheckData(oil);
            oil = _genericServices.Update(oil);
            return oil;
        }

        public void Delete(int id)
        {
            var oil = _genericServices.Read(id);
            if (oil == null)
                throw new EOilNotFound();
            _genericServices.Delete(oil);
        }

        private void CheckData(Oil oil)
        {
            if (oil.name.Length > 50)
                throw new EInvalidName();
            if (oil.calIn14Ml > 300)
                throw new EInvalidCalories();
            if (oil.name.Length < 5)
                throw new EInvalidName();
            if (oil.calIn14Ml < 30)
                throw new EInvalidCalories();
        }
    }
}
