using ExpenseTracker.Server.Data;
using ExpenseTracker.Server.Models;
using ExpenseTracker.Server.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Server.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _context;

        public CurrencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Currency>> GetAllAsync()
        {
            return _context.Currencies
                .Include(x => x.Transactions)
                .Include(x => x.Accounts)
                .ToListAsync();
        }

        public Task<Currency?> GetByIdAsync(int id)
        {
            var currency = _context.Currencies
                .Include(x => x.Transactions)
                .Include(x => x.Accounts)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            return currency;
        }
    }
}
