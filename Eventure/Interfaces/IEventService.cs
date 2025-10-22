using Eventure.Models;

namespace Eventure.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventComponent>> GetAllAsync();
    Task<EventComponent?> GetByIdAsync(int id);
    Task<EventComponent> CreateAsync(EventComponent ev);
    Task<EventComponent?> UpdateAsync(int id, EventComponent ev);
    Task<bool> DeleteAsync(int id);
}