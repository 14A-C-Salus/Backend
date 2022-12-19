using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if !DEBUG
    [Authorize(Roles = "Admin")]
#endif
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [HttpPut("create")]
        public IActionResult Create(TagCreateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_tagService.Create(request));
            });
        }
        [HttpPatch("update")]
        public IActionResult Update(TagUpdateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_tagService.Update(request));
            });
        }
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
