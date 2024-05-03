using ExpenseTracker.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace ExpenseTracker.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendConfirmationEmailAsync(string recipient, string confirmlink)
        {
            var client = new SendGridClient(_configuration["SendGrid:ApiKey"]);
            var from = new EmailAddress("expensetracker624@gmail.com", "Expense Tracker");
            var to = new EmailAddress(recipient);
            var templateId = "d-b79bb7164aa448aeb240521813d02af3";
            var dynamicTemplateData = new
            {
                givenEmail = recipient,
                confirmationLink = confirmlink
            };
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);
            await client.SendEmailAsync(msg);
        }
    }
}
