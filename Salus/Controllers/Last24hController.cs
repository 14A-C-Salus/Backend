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
        [HttpGet("get-all")]
        public IActionResult GetAll(DateTimeOffset date)
        {
            return this.Run(() =>
            {
                return Ok(_last24HService.GetAll());
            });
        }
        [HttpPut("add-new-recipe")]
        public IActionResult AddNewRecipe(AddRecipeToLast24H request)
        {
            return this.Run(() =>
            {
                return Ok(_last24HService.Add(request));
            });
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            return this.Run(() =>
            {
                _last24HService.Delete(id);
                return Ok();
            });
        }

        [HttpPatch("half-portion")]
        public IActionResult HalfPortion(int id)
        {
            return this.Run(() =>
            {
                return Ok(_last24HService.HalfPortion(id));
            });
        }
        [HttpPatch("third-portion")]
        public IActionResult ThirdPortion(int id)
        {
            return this.Run(() =>
            {
                return Ok(_last24HService.ThirdPortion(id));
            });
        }
        [HttpPatch("quarter-portion")]
        public IActionResult QuarterPortion(int id)
        {
            return this.Run(() =>
            {
                return Ok(_last24HService.QuarterPortion(id));
            });
        }
        [HttpPatch("double-portion")]
        public IActionResult DoublePortion(int id)
        {
            return this.Run(() =>
            {
                return Ok(_last24HService.DoublePortion(id));
            });
        }
    }
}
