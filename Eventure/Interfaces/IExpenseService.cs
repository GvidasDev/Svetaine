using Eventure.Dtos.Expenses;

namespace Eventure.Interfaces
{
    public interface IExpenseService
    {
        Task<List<ExpenseDto>> GetExpensesAsync(int eventId, int currentUserId);
        Task<ExpenseDto> AddExpenseAsync(int eventId, int currentUserId, CreateExpenseDto dto);
        Task<List<ExpenseBalanceDto>> GetBalancesAsync(int eventId, int currentUserId);
    }
}
