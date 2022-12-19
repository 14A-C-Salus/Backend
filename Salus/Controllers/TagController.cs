namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if !DEBUG
    [Authorize]
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
    }
}
