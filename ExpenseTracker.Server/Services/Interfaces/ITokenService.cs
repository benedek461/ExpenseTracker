using ExpenseTracker.Server.Models;

namespace ExpenseTracker.Server.Services.Interfaces
{
    public interface ITokenService
    {
        Task<JwtTokens> GenerateJwtTokenAsync(User user);
    }
}