using Eventure.Data;
using Eventure.Interfaces;
using Eventure.Models;
using Eventure.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Eventure.Services
{
    public class TaskService : ITaskService
    {
        private readonly DatabaseContext _db;

        public TaskService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<TaskDto>> GetAllAsync(int userId)
        {
            return await _db.Tasks
                .Where(t => t.UserId == userId)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    TaskStatus = t.TaskStatus,
                    EventId = t.EventId,
                    UserId = t.UserId,
                    EventTitle = t.EventId == null ? null : _db.Events.Where(e => e.Id == t.EventId).Select(e => e.Title).FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateAsync(int id, TaskItem dto, int userId)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null) throw new Exception("Not found");

            int? oldEventId = task.EventId;
            int? newEventId = dto.EventId;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.EventId = dto.EventId;

            if (oldEventId == newEventId)
            {
                task.TaskStatus = dto.TaskStatus;
            }
            else
            {
                task.TaskStatus = await EnsureEventColumnAsync(userId, newEventId);
            }

            await _db.SaveChangesAsync();
            return task;
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null) throw new Exception("Not found");

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
        }

        private async Task<int> EnsureEventColumnAsync(int userId, int? eventId)
        {
            if (eventId == null) return 0;

            var existing = await _db.TaskStatuses.FirstOrDefaultAsync(s => s.UserId == userId && s.EventId == eventId);
            if (existing != null) return existing.Id;

            string title = await _db.Events
                .Where(e => e.Id == eventId)
                .Select(e => e.Title)
                .FirstOrDefaultAsync() ?? "Event";

            TaskState st = new TaskState();
            st.UserId = userId;
            st.EventId = eventId;
            st.Name = title;

            _db.TaskStatuses.Add(st);
            await _db.SaveChangesAsync();

            return st.Id;
        }
    }
}
