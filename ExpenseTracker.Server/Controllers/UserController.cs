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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetAllAsync()
        {
            var response = await _userService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetByIdAsync(string id)
        {
            var response = await _userService.GetByIdAsync(id);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost()]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> CreateAsync(CreateUserDto dto)
        {
            var response = await _userService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> ConfirmEmailAsync([FromQuery] string id, [FromQuery] string confirmationToken)
        {
            var response = await _userService.ConfirmEmailAsync(id, confirmationToken);

            if (!response.IsSuccess) 
            { 
                return BadRequest(response); 
            }

            return Redirect("/RedirectToEmailConfirmedPage.html");
        }

        [HttpPatch]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> UpdateAsync(UpdateUserDto dto, string id)
        {
            var response = await _userService.UpdateAsync(dto, id);

            if (!response.IsSuccess)
            {
                if (response.Errors!.Any(e => e.Equals("User was not found!")))
                {
                    return NotFound(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }

            return Ok(response);
        }

        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> DeleteAsync(string id)
        {
            var response = await _userService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                if (response.Errors!.Any(e => e.Equals("User was not found!")))
                {
                    return NotFound(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }

            return Ok(response);
        }
    }
}
