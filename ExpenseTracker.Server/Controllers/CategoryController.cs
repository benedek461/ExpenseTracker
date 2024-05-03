using ExpenseTracker.Server.Responses;
using ExpenseTracker.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetAllAsync()
        {
            var response = await _categoryService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetByIdAsync(int id)
        {
            var response = await _categoryService.GetByIdAsnyc(id);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
