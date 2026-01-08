using Eventure.Data;
using Eventure.Dtos;
using Eventure.Interfaces;
using Eventure.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventure.Services
{
    public class TaskStateService : ITaskStateService
    {
        private readonly DatabaseContext _db;

        public TaskStateService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<TaskStateDto>> GetAllAsync(int userId)
        {
            await EnsureEventColumnsAsync(userId);

            var list = await (from s in _db.TaskStatuses
                              where s.UserId == userId
                              orderby s.Id
                              select new TaskStateDto
                              {
                                  Id = s.Id,
                                  Name = s.Name,
                                  EventId = s.EventId,
                                  EventTitle = s.EventId == null
                                      ? null
                                      : (from e in _db.Events
                                         where e.Id == s.EventId
                                         select e.Title).FirstOrDefault()
                              }).ToListAsync();

            return list;
        }

        public async Task<TaskState?> GetByIdAsync(int id, int userId)
        {
            var state = await (from s in _db.TaskStatuses
                               where s.Id == id && s.UserId == userId
                               select s).FirstOrDefaultAsync();

            return state;
        }

        public async Task<TaskState> CreateAsync(TaskState state, int userId)
        {
            TaskState s = new TaskState();
            s.Name = state.Name;
            s.UserId = userId;
            s.EventId = null;

            _db.TaskStatuses.Add(s);
            await _db.SaveChangesAsync();
            return s;
        }

        public async Task<TaskState> UpdateAsync(int id, TaskState state, int userId)
        {
            var existing = await (from s in _db.TaskStatuses
                                  where s.Id == id && s.UserId == userId
                                  select s).FirstOrDefaultAsync();

            if (existing == null)
            {
                throw new Exception("TaskState not found");
            }

            existing.Name = state.Name;
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var state = await (from s in _db.TaskStatuses
                               where s.Id == id && s.UserId == userId
                               select s).FirstOrDefaultAsync();

            if (state == null)
            {
                throw new Exception("TaskState not found");
            }

            bool hasTasks = await (from t in _db.Tasks
                                   where t.UserId == userId && t.TaskStatus == id
                                   select t.Id).AnyAsync();

            if (hasTasks)
            {
                throw new Exception("Cannot delete column with tasks.");
            }

            _db.TaskStatuses.Remove(state);
            await _db.SaveChangesAsync();
        }

        private async Task EnsureEventColumnsAsync(int userId)
        {
            var myEventIds = await (from e in _db.Events
                                    where e.UserId == userId
                                    select e.Id).ToListAsync();

            var invitedEventIds = await (from i in _db.EventInvitations
                                         where i.UserId == userId
                                         select i.EventId).ToListAsync();

            var allEventIds = myEventIds.Concat(invitedEventIds).Distinct().ToList();

            for (int i = 0; i < allEventIds.Count; i++)
            {
                int eventId = allEventIds[i];

                var existingState = await (from s in _db.TaskStatuses
                                           where s.UserId == userId && s.EventId == eventId
                                           select s).FirstOrDefaultAsync();

                if (existingState != null)
                {
                    continue;
                }

                string title = await (from e in _db.Events
                                      where e.Id == eventId
                                      select e.Title).FirstOrDefaultAsync() ?? "Event";

                TaskState st = new TaskState();
                st.UserId = userId;
                st.EventId = eventId;
                st.Name = title;

                _db.TaskStatuses.Add(st);
                await _db.SaveChangesAsync();
            }
        }
    }
}
