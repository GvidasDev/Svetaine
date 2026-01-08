namespace Eventure.Dtos.Expenses
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public string Note { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
