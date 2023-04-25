using Salus.Models.Requests;
using Salus.Services;
using Salus.WebAPI;

namespace Salus.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DietController : Controller
    {
        private readonly IDietService _dietService;
        public DietController(IDietService dietService)
        {
            _dietService = dietService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("create")]
        public IActionResult Create(CreateDietRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_dietService.Create(request));
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("update")]
        public IActionResult Update(ModifyDietRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_dietService.Modify(request));
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            return this.Run(() =>
            {
                _dietService.Delete(id);
                return Ok();
            });
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return this.Run(() =>
            {
                return Ok(_dietService.GetAll());
            });
        }
    }
}
