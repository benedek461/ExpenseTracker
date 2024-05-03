using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Responses;

namespace ExpenseTracker.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<CommonResponse> LoginAsync(LoginDto dto);
        Task<CommonResponse> RefreshAsync(string refreshToken, string id);
    }
}