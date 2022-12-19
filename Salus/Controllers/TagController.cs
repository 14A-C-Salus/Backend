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
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly ITagService _tagService;
        public TagController(DataContext dataContext, IConfiguration configuration, ITagService tagService)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _tagService = tagService;
        }
        [HttpPut("create")]
        public IActionResult Create(TagCreateRequest tag)
        {
            return this.Run(() =>
            {
                return Ok(_tagService.Create(tag));
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
