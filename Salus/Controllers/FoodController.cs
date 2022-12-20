﻿using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if !DEBUG
    [Authorize]
#endif
    public class FoodController:Controller
    {
        private readonly IFoodService _foodService;
        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }
        [HttpPut("create")]
        public IActionResult Create(FoodCreateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_foodService.Create(request));
            });
        }
        [HttpPatch("update")]
        public IActionResult Update(FoodUpdateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_foodService.Update(request));
            });
        }
#if !DEBUG
    [Authorize(Roles = "Admin")]
#endif
        [HttpPatch("verify")]
        public IActionResult Verify(int id)
        {
            return this.Run(() =>
            {
                return Ok(_foodService.VerifyUnVerify(id));
            });
        }
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            return this.Run(() =>
            {
                _foodService.Delete(id);
                return Ok();
            });
        }
        [HttpPatch("add-tags")]
        public IActionResult AddTags(AddTagsToFoodRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_foodService.AddTags(request));
            });
        }
    }
}
