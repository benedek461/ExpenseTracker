using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Responses;
using ExpenseTracker.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> LoginAsync(LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);

            if (!response.IsSuccess)
            {
                switch (response.Errors?.FirstOrDefault())
                {
                    case "No user found under this username!":
                        return NotFound(response);
                    default:
                        return BadRequest(response);
                }
            }

            return Ok(response);
        }

        [HttpPost("Refresh")]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> RefreshAsync([FromQuery] string id, [FromQuery] string refreshToken)
        {
            var response = await _authService.RefreshAsync(id, refreshToken);

            if (!response.IsSuccess)
            {
                switch (response.Errors?.FirstOrDefault())
                {
                    case "User was not found!":
                        return NotFound(response);
                    default:
                        return BadRequest(response);
                }
            }

            return Ok(response);
        }
    }
}
