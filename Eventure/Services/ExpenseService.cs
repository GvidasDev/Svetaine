using Eventure.Data;
using Eventure.Dtos.Expenses;
using Eventure.Helpers;
using Eventure.Interfaces;
using Eventure.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventure.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly DatabaseContext _db;

        public ExpenseService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<ExpenseDto>> GetExpensesAsync(int eventId, int currentUserId)
        {
            bool ok = await CanAccessEventAsync(eventId, currentUserId);
            if (!ok)
            {
                throw new Exception("Forbidden");
            }

            var list = await (from x in _db.EventExpenses
                              where x.EventId == eventId
                              orderby x.CreatedAt descending, x.Id descending
                              select new ExpenseDto
                              {
                                  Id = x.Id,
                                  EventId = x.EventId,
                                  UserId = x.UserId,
                                  Username = (from u in _db.Users where u.Id == x.UserId select u.Username).FirstOrDefault() ?? "Unknown",
                                  Amount = x.Amount,
                                  Note = x.Note,
                                  CreatedAt = x.CreatedAt
                              }).ToListAsync();

            return list;
        }

        public async Task<ExpenseDto> AddExpenseAsync(int eventId, int currentUserId, CreateExpenseDto dto)
        {
            bool ok = await CanAccessEventAsync(eventId, currentUserId);
            if (!ok)
            {
                throw new Exception("Forbidden");
            }

            decimal amount = dto.Amount;
            if (amount <= 0m)
            {
                throw new Exception("Invalid amount");
            }

            string note = (dto.Note ?? "").Trim();

            EventExpense e = new EventExpense();
            e.EventId = eventId;
            e.UserId = currentUserId;
            e.Amount = amount;
            e.Note = note;
            e.CreatedAt = DateTime.UtcNow;

            _db.EventExpenses.Add(e);
            await _db.SaveChangesAsync();

            ExpenseDto outDto = new ExpenseDto();
            outDto.Id = e.Id;
            outDto.EventId = e.EventId;
            outDto.UserId = e.UserId;
            outDto.Username = await (from u in _db.Users where u.Id == currentUserId select u.Username).FirstOrDefaultAsync() ?? "Unknown";
            outDto.Amount = e.Amount;
            outDto.Note = e.Note;
            outDto.CreatedAt = e.CreatedAt;

            return outDto;
        }

        public async Task<List<ExpenseBalanceDto>> GetBalancesAsync(int eventId, int currentUserId)
        {
            bool ok = await CanAccessEventAsync(eventId, currentUserId);
            if (!ok)
            {
                throw new Exception("Forbidden");
            }

            var participants = await GetParticipantsAsync(eventId);

            var expenses = await (from x in _db.EventExpenses
                                  where x.EventId == eventId
                                  select new ExpenseSplitHelper.ExpenseMini
                                  {
                                      UserId = x.UserId,
                                      Amount = x.Amount
                                  }).ToListAsync();

            var list = ExpenseSplitHelper.BuildBalances(participants, expenses);
            return list;
        }

        private async Task<bool> CanAccessEventAsync(int eventId, int userId)
        {
            bool isOwner = await _db.Events.AnyAsync(e => e.Id == eventId && e.UserId == userId);

            if (isOwner)
            {
                return true;
            }

            bool isInvited = await _db.EventInvitations.AnyAsync(i => i.EventId == eventId && i.UserId == userId);

            return isInvited;
        }

        private async Task<List<ExpenseSplitHelper.UserMini>> GetParticipantsAsync(int eventId)
        {
            int ownerId = await (from e in _db.Events where e.Id == eventId select e.UserId).FirstOrDefaultAsync();

            List<int> ids = new List<int>();
            if (ownerId != 0)
            {
                ids.Add(ownerId);
            }

            var invitedIds = await (from i in _db.EventInvitations
                                    where i.EventId == eventId
                                    select i.UserId).ToListAsync();

            for (int i = 0; i < invitedIds.Count; i++)
            {
                if (!ids.Contains(invitedIds[i]))
                {
                    ids.Add(invitedIds[i]);
                }
            }

            var users = await (from u in _db.Users
                               where ids.Contains(u.Id)
                               select new ExpenseSplitHelper.UserMini
                               {
                                   Id = u.Id,
                                   Username = u.Username
                               }).ToListAsync();

            return users;
        }
    }
}
