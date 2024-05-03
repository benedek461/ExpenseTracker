using ExpenseTracker.Server.Models;

namespace ExpenseTracker.Server.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> GetAllAsync();
        Task<Currency?> GetByIdAsync(int id);
    }
}