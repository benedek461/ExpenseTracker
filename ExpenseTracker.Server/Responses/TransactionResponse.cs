namespace ExpenseTracker.Server.Responses
{
    public class TransactionResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int CategoryId { get; set; }
        public float Amount { get; set; }
        public int CurrencyId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
