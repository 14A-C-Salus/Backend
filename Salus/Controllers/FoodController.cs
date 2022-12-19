namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if !DEBUG
    [Authorize]
#endif
    public class FoodController:Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IFoodService _foodService;
        public FoodController(DataContext dataContext, IConfiguration configuration, IFoodService foodService)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _foodService = foodService;
        }
    }
}
