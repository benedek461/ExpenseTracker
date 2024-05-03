using ExpenseTracker.Server.Responses;

namespace ExpenseTracker.Server.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByIdAsync(int id);
    }
}