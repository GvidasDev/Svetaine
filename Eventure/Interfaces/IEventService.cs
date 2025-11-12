using Eventure.Models;
using Eventure.Dtos;

namespace Eventure.Interfaces;

public interface IEventService
{
        Task<IEnumerable<EventDto>> GetAllAsync();
        Task<EventDto?> GetByIdAsync(int id);
        Task<EventComponent> CreateAsync(EventComponent ev);
        Task<EventComponent?> UpdateAsync(int id, EventComponent ev);
        Task<bool> DeleteAsync(int id);
}