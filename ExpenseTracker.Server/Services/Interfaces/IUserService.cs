using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Responses;

namespace ExpenseTracker.Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<CommonResponse> ConfirmEmailAsync(string id, string confirmationToken);
        Task<CommonResponse> CreateAsync(CreateUserDto dto);
        Task<CommonResponse> DeleteAsync(string id);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByIdAsync(string id);
        Task<CommonResponse> UpdateAsync(UpdateUserDto dto, string id);
    }
}