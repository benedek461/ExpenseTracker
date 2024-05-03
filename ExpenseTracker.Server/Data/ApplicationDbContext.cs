using ExpenseTracker.Server.Enums;
using ExpenseTracker.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Currency)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CurrencyId);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithOne(u => u.Account)
                .HasForeignKey<Account>(a => a.UserId);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Currency)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.CurrencyId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Title = "Healthcare", Icon = "❤️", Type = CategoryType.Cost },
                new Category { Id = 2, Title = "Freetime", Icon = "🏔️", Type = CategoryType.Cost },
                new Category { Id = 3, Title = "Home", Icon = "🏠", Type = CategoryType.Cost },
                new Category { Id = 4, Title = "Restaurant", Icon = "🍴", Type = CategoryType.Cost },
                new Category { Id = 5, Title = "Education", Icon = "🎓", Type = CategoryType.Cost },
                new Category { Id = 6, Title = "Presents", Icon = "🎁", Type = CategoryType.Cost },
                new Category { Id = 7, Title = "Groceries", Icon = "🛒", Type = CategoryType.Cost },
                new Category { Id = 8, Title = "Family", Icon = "👨‍👩‍👧", Type = CategoryType.Cost },
                new Category { Id = 9, Title = "Transport", Icon = "🚍", Type = CategoryType.Cost },
                new Category { Id = 10, Title = "Gym", Icon = "💪", Type = CategoryType.Cost },
                new Category { Id = 11, Title = "Other", Icon = "❓", Type = CategoryType.Cost },

                new Category { Id = 12, Title = "Paycheck", Icon = "💵", Type = CategoryType.Income },
                new Category { Id = 13, Title = "Present", Icon = "🎁", Type = CategoryType.Income },
                new Category { Id = 14, Title = "Other", Icon = "❓", Type = CategoryType.Income }
            );

            modelBuilder.Entity<Currency>().HasData(
                new Currency { Id = 1, Name = "Euró", Code = "EUR" },
                new Currency { Id = 2, Name = "Angol Font", Code = "GBP" },
                new Currency { Id = 3, Name = "Magyar Forint", Code = "HUF" },
                new Currency { Id = 4, Name = "Amerikai Dollár", Code = "USD" }
            );
        }
    }
}
