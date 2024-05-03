using System.ComponentModel;

namespace ExpenseTracker.Server.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int CategoryId { get; set; }
        public float Amount { get; set; }
        public int CurrencyId { get; set; }
        public string Description { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public Currency Currency { get; set; } = null!;
    }
}
