using Salus.Models.Requests;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Last24hController : Controller
    {
        private readonly ILast24hService _last24HService;
        public Last24hController(ILast24hService last24HService)
        {
            _last24HService = last24HService;
        }
        [HttpPut("add-new-food")]
        public IActionResult AddNewFood(AddFoodToLast24H request)
        {
            return this.Run(() =>
            {
                return Ok(_last24HService.Add(request));
            });
        }
    }
}
