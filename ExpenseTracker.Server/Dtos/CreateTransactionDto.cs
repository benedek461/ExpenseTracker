namespace ExpenseTracker.Server.Dtos
{
    public class CreateTransactionDto
    {
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public float Amount { get; set; }
        public int CurrencyId { get; set; }
    }
}
