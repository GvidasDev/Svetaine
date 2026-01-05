using Eventure.Data;
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

        public async Task<List<TaskState>> GetAllAsync()
        {
            return await _db.TaskStatuses.OrderBy(s => s.Id).ToListAsync();
        }

        public async Task<TaskState?> GetByIdAsync(int id)
        {
            return await _db.TaskStatuses.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<TaskState> CreateAsync(TaskState state)
        {
            _db.TaskStatuses.Add(state);
            await _db.SaveChangesAsync();
            return state;
        }

        public async Task<TaskState> UpdateAsync(int id, TaskState state)
        {
            var existing = await _db.TaskStatuses.FirstOrDefaultAsync(s => s.Id == id);
            if (existing == null) throw new Exception("TaskState not found");

            existing.Name = state.Name;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var state = await _db.TaskStatuses.FirstOrDefaultAsync(s => s.Id == id);
            if (state == null) throw new Exception("TaskState not found");

            _db.TaskStatuses.Remove(state);
            await _db.SaveChangesAsync();
        }
    }
}
