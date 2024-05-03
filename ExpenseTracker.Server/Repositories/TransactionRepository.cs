using ExpenseTracker.Server.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Server.Models;
using ExpenseTracker.Server.Repositories.Interfaces;

namespace ExpenseTracker.Server.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Transaction>> GetAllAsync()
        {
            return _context.Transactions
                .Include(x => x.Category)
                .Include(x => x.Currency)
                .ToListAsync();
        }

        public Task<Transaction?> GetByIdAsync(int id)
        {
            var transaction = _context.Transactions
                .Include(x => x.Category)
                .Include(x => x.Currency)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            return transaction;
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> DeleteAsync(int id)
        {
            var transaction = await _context.Transactions
                .Include(x => x.Category)
                .Include(x => x.Currency)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }
    }
}
