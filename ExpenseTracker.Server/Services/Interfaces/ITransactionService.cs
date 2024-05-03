using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Responses;

namespace ExpenseTracker.Server.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<CommonResponse> CreateAsync(CreateTransactionDto dto);
        Task<CommonResponse> DeleteAsync(int id);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> UpdateAsync(int id, UpdateTransactionDto dto);
    }
}