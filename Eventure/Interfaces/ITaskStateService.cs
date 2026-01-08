using Eventure.Dtos;
using Eventure.Models;

namespace Eventure.Interfaces
{
    public interface ITaskStateService
    {
        Task<List<TaskStateDto>> GetAllAsync(int userId);
        Task<TaskState?> GetByIdAsync(int id, int userId);
        Task<TaskState> CreateAsync(TaskState state, int userId);
        Task<TaskState> UpdateAsync(int id, TaskState state, int userId);
        Task DeleteAsync(int id, int userId);
    }
}
