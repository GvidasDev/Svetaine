namespace Eventure.Dtos.Expenses
{
    public class ExpenseBalanceDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;

        public decimal Paid { get; set; }
        public decimal Share { get; set; }
        public decimal Owes { get; set; }
    }
}
