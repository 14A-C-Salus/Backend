﻿using Salus.Services;
using Salus.WebAPI;

namespace Salus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [Authorize(Roles = "User")]
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return this.Run(() =>
            {
                return Ok(_tagService.GetAll());
            });
        }
        [HttpPut("create")]
        public IActionResult Create(TagCreateRequest request)
        {
            return this.Run(() =>
            {
                return Ok(_tagService.Create(request));
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
