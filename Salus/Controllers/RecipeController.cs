using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IRecipeService _recipeService;
        public RecipeController(DataContext dataContext, IConfiguration configuration, IRecipeService recipeService)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _recipeService = recipeService;
        }
    }
}
