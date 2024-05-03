namespace ExpenseTracker.Server.Responses
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int CurrencyId { get; set; }
        public float Balance { get; set; }
    }
}
