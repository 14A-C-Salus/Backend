using Salus.Models;
using Salus.Models.Requests;

namespace Salus.Services
{
    public interface IDietService
    {
        Diet? Create(CreateDietRequest request);
        Diet? Modify(ModifyDietRequest request);
        void Delete(int id);
    }
}
