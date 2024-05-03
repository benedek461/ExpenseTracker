using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Server.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int AccountId { get; set; }
        public int TransactionId { get; set; }
        public Account Account { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
