namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if !DEBUG
    [Authorize]
#endif
    public class OilController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IOilService _oilService;
        public OilController(DataContext dataContext, IConfiguration configuration, IOilService oilService)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _oilService = oilService;
        }
    }
}
