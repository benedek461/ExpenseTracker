using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Responses;
using ExpenseTracker.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetAllAsync()
        {
            var response = await _accountService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetByIdAsync(int id)
        {
            var response = await _accountService.GetByIdAsync(id);

            if (!response.IsSuccess)
            {
                switch (response.Errors?.FirstOrDefault())
                {
                    case "Account was not found!":
                        return NotFound(response);
                    default:
                        return BadRequest(response);
                }
            }

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> CreateAsync(CreateAccountDto dto)
        {
            var response = await _accountService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                switch (response.Errors?.FirstOrDefault())
                {
                    case "User was not found!":
                    case "Currency was not found!":
                        return NotFound(response);
                    default:
                        return BadRequest(response);
                }
            }

            return Ok(response);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> UpdateAsync(int id, UpdateAccountDto dto)
        {
            var response = await _accountService.UpdateAsync(id, dto);

            if (!response.IsSuccess)
            {
                switch (response.Errors?.FirstOrDefault())
                {
                    case "Account was not found!":
                        return NotFound(response);
                    default:
                        return BadRequest(response);
                }
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> DeleteAsync(int id)
        {
            var response = await _accountService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
