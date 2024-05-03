using ExpenseTracker.Server.Responses;
using ExpenseTracker.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetAllAsync()
        {
            var response = await _currencyService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetByIdAsync(int id)
        {
            var response = await _currencyService.GetByIdAsync(id);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
