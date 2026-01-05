using Eventure.Models;
using Eventure.Dtos;

namespace Eventure.Interfaces;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllAsync(int userId);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(int id, TaskItem dto, int userId);
    Task DeleteAsync(int id, int userId);
}