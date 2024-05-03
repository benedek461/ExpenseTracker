namespace ExpenseTracker.Server.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string recipient, string confirmlink);
    }
}