using ExpenseTracker.Server.Enums;

namespace ExpenseTracker.Server.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public CategoryType Type { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}