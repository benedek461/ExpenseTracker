namespace ExpenseTracker.Server.Dtos
{
    public class UpdateTransactionDto
    {
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public float Amount { get; set; }
    }
}
