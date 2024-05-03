namespace ExpenseTracker.Server.Dtos
{
    public class CreateAccountDto
    {
        public string UserId { get; set; } = string.Empty;
        public int CurrencyId { get; set; }
        public float Balance { get; set; }
    }
}
