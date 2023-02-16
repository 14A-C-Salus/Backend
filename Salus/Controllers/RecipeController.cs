using Salus.Services;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpPut("create")]
        public IActionResult Create(WriteRecipeRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.WriteRecipe(request));
            });
        }

        [HttpPut("update")]
        public IActionResult Update(UpdateRecipeRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.Update(request));
            });
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            return this.Run(() =>
            {
                _recipeService.Delete(id);
                return Ok();
            });
        }
    }
}
