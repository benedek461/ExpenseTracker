namespace ExpenseTracker.Server.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int CurrencyId { get; set; }
        public float Balance { get; set; }

        public User User { get; set; } = null!;
        public Currency Currency { get; set; } = null!;
    }
}
