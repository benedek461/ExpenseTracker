using ExpenseTracker.Server.Data;
using ExpenseTracker.Server.Models;
using ExpenseTracker.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Server.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Category>> GetAllAsync()
        {
            return _context.Categories
                .Include(x => x.Transactions)
                .ToListAsync();
        }

        public Task<Category?> GetByIdAsync(int id)
        {
            var category = _context.Categories
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            return category;
        }
    }
}
