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
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetAllAsync()
        {
            var response = await _transactionService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> GetByIdAsnyc(int id)
        {
            var response = await _transactionService.GetByIdAsync(id);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(CommonResponse))]
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> CreateAsync(CreateTransactionDto dto)
        {
            var response = await _transactionService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                switch (response.Errors?.FirstOrDefault())
                {
                    case "User was not found!":
                    case "Transaction was not found!":
                    case "Category was not found!":
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
        [ProducesResponseType(400, Type = typeof(CommonResponse))]
        [ProducesResponseType(404, Type = typeof(CommonResponse))]
        public async Task<ActionResult<CommonResponse>> UpdateAsync(int id, UpdateTransactionDto dto)
        {
            var response = await _transactionService.UpdateAsync(id, dto);

            if (!response.IsSuccess)
            {
                switch (response.Errors?.FirstOrDefault())
                {
                    case "Transaction was not found!":
                    case "Category was not found!":
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
            var response = await _transactionService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
