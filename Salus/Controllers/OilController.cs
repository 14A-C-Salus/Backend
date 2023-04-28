using Salus.Services;
using Salus.WebAPI;

namespace Salus.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OilController : Controller
    {
        private readonly IOilService _oilService;
        public OilController(IOilService oilService)
        {
            _oilService = oilService;
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return this.Run(() =>
            {
                return Ok(_oilService.GetAll());
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("create")]
        public IActionResult Create(OilCreateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_oilService.Create(request));
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("update")]
        public IActionResult Update(OilUpdateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_oilService.Update(request));
            });
        }
        [Authorize(Roles = "Admin")]
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
