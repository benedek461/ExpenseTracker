using ExpenseTracker.Server.Responses;

namespace ExpenseTracker.Server.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByIdAsnyc(int id);
    }
}