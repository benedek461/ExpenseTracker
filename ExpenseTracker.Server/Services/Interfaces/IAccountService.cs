using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Responses;

namespace ExpenseTracker.Server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<CommonResponse> CreateAsync(CreateAccountDto dto);
        Task<CommonResponse> DeleteAsync(int id);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> UpdateAsync(int id, UpdateAccountDto dto);
    }
}