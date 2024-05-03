namespace ExpenseTracker.Server.Responses
{
    public class CurrencyResponse
    {
        public int Id {  get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
