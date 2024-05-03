using ExpenseTracker.Server.Data;
using ExpenseTracker.Server.Models;
using ExpenseTracker.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Server.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Account>> GetAllAsync()
        {
            return _context.Accounts
                .Include(a => a.User)
                .Include(a => a.Currency)
                .ToListAsync();
        }

        public Task<Account?> GetByIdAsync(int id)
        {
            var account = _context.Accounts
                .Include(a => a.User)
                .Include(a => a.Currency)
                .FirstOrDefaultAsync(a => a.Id.Equals(id));

            return account;
        }

        public async Task<Account> CreateAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Account> DeleteAsync(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.User)
                .Include(a => a.Currency)
                .FirstOrDefaultAsync(a => a.Id.Equals(id));

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return account;
        }
    }
}
