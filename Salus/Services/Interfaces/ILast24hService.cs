using Salus.Models.Requests;

namespace Salus.Services.Last24hServices
{
    public interface ILast24hService
    {
        public Last24h Add(AddFoodToLast24H request);
    }
}
