using Salus.Services;
using Salus.WebAPI;

namespace Salus.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OilController : Controller
    {
        private readonly IOilService _oilService;
        public OilController(IOilService oilService)
        {
            _oilService = oilService;
        }
        [HttpPut("create")]
        public IActionResult Create(OilCreateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_oilService.Create(request));
            });
        }
        [HttpPatch("update")]
        public IActionResult Update(OilUpdateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_oilService.Update(request));
            });
        }
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            return this.Run(() =>
            {
                _oilService.Delete(id);
                return Ok();
            });
        }
    }
}
