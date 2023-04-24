using Salus.Models;
using Salus.Models.Requests;

namespace Salus.Services.Interfaces
{
    public interface IDietService
    {
        Diet? Create(CreateDietRequest request);
        Diet? Modify(ModifyDietRequest request);
        void Delete(int id);
        List<Diet> GetAll();
    }
}
