using ExpenseTracker.Server.Models;

namespace ExpenseTracker.Server.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<Transaction> DeleteAsync(int id);
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task UpdateAsync();
    }
}