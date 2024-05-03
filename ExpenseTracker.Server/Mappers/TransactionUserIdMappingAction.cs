using AutoMapper;
using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ExpenseTracker.Server.Mappers
{
    public class TransactionUserIdMappingAction : IMappingAction<CreateTransactionDto, Transaction>
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public TransactionUserIdMappingAction(
            UserManager<User> userManager, 
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public void Process(CreateTransactionDto source, Transaction destination, ResolutionContext context)
        {
            var myId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            destination.UserId = myId;
        }
    }
}
