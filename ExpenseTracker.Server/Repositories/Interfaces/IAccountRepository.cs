using ExpenseTracker.Server.Models;

namespace ExpenseTracker.Server.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> CreateAsync(Account account);
        Task<Account> DeleteAsync(int id);
        Task<List<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int id);
        Task UpdateAsync();
    }
}