namespace Eventure.Dtos.Expenses
{
    public class CreateExpenseDto
    {
        public decimal Amount { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
