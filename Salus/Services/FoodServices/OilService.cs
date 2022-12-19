using Salus.Controllers.Models.FoodModels;
using System.Security.Cryptography;

namespace Salus.Services.FoodServices
{
    public class OilService:IOilService
    {
        private readonly DataContext _dataContext;
        private readonly CRUD<Oil> _crud;
        public OilService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _crud = new CRUD<Oil>(_dataContext);
        }

        public Oil Create(OilCreateRequest request)
        {
            var oil = new Oil()
            {
                name = request.name,
                calIn14Ml = request.calIn14Ml,
            };
            CheckData(oil);
            oil = _crud.Create(oil);
            return oil;
        }

        public Oil Update(OilUpdateRequest request)
        {
            var oil = _crud.Read(request.id);
            if (oil == null)
                throw new Exception("This oil doesn't exist.");

            oil.name = request.name.Length == 0 ? oil.name : request.name;
            oil.calIn14Ml = request.calIn14Ml == 0 ? oil.calIn14Ml : request.calIn14Ml;
            CheckData(oil);
            oil = _crud.Update(oil);
            return oil;
        }

        public void Delete(int id)
        {
            var oil = _crud.Read(id);
            if (oil == null)
                throw new Exception("This oil doesn't exist.");
            _crud.Delete(oil);
        }

        protected void CheckData(Oil oil)
        {
            if (oil.name.Length > 50)
                throw new Exception("Name can't be longer then 50 character!");
            if (oil.calIn14Ml > 300)
                throw new Exception("Oils can't contains more then 300 cal/14ml!");
            if (oil.name.Length < 5)
                throw new Exception("Please enter at least 5 character to the name field.");
            if (oil.calIn14Ml < 30)
                throw new Exception("Oils can't contains less then 30 cal/14ml");
        }
    }
}
