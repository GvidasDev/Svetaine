using Eventure.Dtos.Expenses;

namespace Eventure.Helpers
{
    public static class ExpenseSplitHelper
    {
        public static List<ExpenseBalanceDto> BuildBalances(
            List<UserMini> participants,
            List<ExpenseMini> expenses)
        {
            int count = participants.Count;
            if (count == 0)
            {
                return new List<ExpenseBalanceDto>();
            }

            decimal total = 0m;
            Dictionary<int, decimal> paidByUser = new Dictionary<int, decimal>();

            for (int i = 0; i < expenses.Count; i++)
            {
                total += expenses[i].Amount;

                if (!paidByUser.ContainsKey(expenses[i].UserId))
                {
                    paidByUser[expenses[i].UserId] = 0m;
                }

                paidByUser[expenses[i].UserId] = paidByUser[expenses[i].UserId] + expenses[i].Amount;
            }

            decimal share = total / count;

            List<ExpenseBalanceDto> list = new List<ExpenseBalanceDto>();

            for (int i = 0; i < participants.Count; i++)
            {
                UserMini p = participants[i];

                decimal paid = 0m;
                if (paidByUser.ContainsKey(p.Id))
                {
                    paid = paidByUser[p.Id];
                }

                decimal owes = share - paid;
                if (owes < 0m)
                {
                    owes = 0m;
                }

                ExpenseBalanceDto dto = new ExpenseBalanceDto();
                dto.UserId = p.Id;
                dto.Username = p.Username;
                dto.Paid = paid;
                dto.Share = share;
                dto.Owes = owes;

                list.Add(dto);
            }

            list = list.OrderByDescending(x => x.Owes).ToList();
            return list;
        }

        public class UserMini
        {
            public int Id { get; set; }
            public string Username { get; set; } = string.Empty;
        }

        public class ExpenseMini
        {
            public int UserId { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
