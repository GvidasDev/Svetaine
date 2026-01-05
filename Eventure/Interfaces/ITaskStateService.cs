using Eventure.Models;

namespace Eventure.Interfaces
{
    public interface ITaskStateService
    {
        Task<List<TaskState>> GetAllAsync();
        Task<TaskState?> GetByIdAsync(int id);
        Task<TaskState> CreateAsync(TaskState state);
        Task<TaskState> UpdateAsync(int id, TaskState state);
        Task DeleteAsync(int id);
    }
}
