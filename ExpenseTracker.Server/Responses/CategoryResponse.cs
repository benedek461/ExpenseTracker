using ExpenseTracker.Server.Enums;

namespace ExpenseTracker.Server.Responses
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public CategoryType Type { get; set; }
    }
}
