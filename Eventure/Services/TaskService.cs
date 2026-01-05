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
                    UserId = t.UserId
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

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.TaskStatus = dto.TaskStatus;
            task.EventId = dto.EventId;

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
    }

}