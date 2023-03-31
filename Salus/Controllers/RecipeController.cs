﻿using Salus.Services;
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

        [HttpGet("get-recommended-tags")]
        public IActionResult GetRecommendedTags(int recipeId)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.GetRecommendedTags(recipeId));
            });
        }

        [HttpPut("create-simple")]
        public IActionResult CreateSimple(RecipeCreateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.CreateSimple(request));
            });
        }

        [HttpPatch("update")]
        public IActionResult UpdateSimple(RecipeUpdateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.UpdateSimple(request));
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("verify")]
        public IActionResult Verify(int id)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.VerifyUnVerify(id));
            });
        }

        [HttpPatch("add-tags")]
        public IActionResult AddTags(AddTagsToRecipeRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.AddTags(request));
            });
        }

        [HttpPatch("like-unlike")]
        public IActionResult LikeUnlike(int recipeId)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.LikeUnlike(recipeId));
            });
        }

        [HttpGet("get-all-recipe-by-auth-id")]
        public IActionResult GetAllRecipe(int authId)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.GetAll(authId));
            });
        }

        [HttpPut("create")]
        public IActionResult Create(WriteRecipeRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_recipeService.Create(request));
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
