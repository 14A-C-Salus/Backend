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
    }
}
