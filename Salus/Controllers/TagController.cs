using Microsoft.OpenApi.Any;
using Salus.Services;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return this.Run(() =>
            {
                return Ok(_tagService.GetAll());
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("create")]
        public IActionResult Create(TagCreateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_tagService.Create(request));
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("update")]
        public IActionResult Update(TagUpdateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_tagService.Update(request));
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            return this.Run(() =>
            {
                _tagService.Delete(id);
                return Ok();
            });
        }
    }
}
