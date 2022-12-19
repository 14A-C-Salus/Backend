namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if !DEBUG
    [Authorize]
#endif
    public class Last24hController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly ILast24hService _last24HService;
        public Last24hController(DataContext dataContext, IConfiguration configuration, ILast24hService last24HService)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _last24HService = last24HService;
        }
    }
}
