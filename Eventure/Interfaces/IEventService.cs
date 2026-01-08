using Eventure.Dtos;
using Eventure.Models;

namespace Eventure.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllAsync(int userId);
        Task<EventDto?> GetByIdAsync(int id, int userId);
        Task<EventComponent> CreateAsync(EventComponent ev);
        Task<EventComponent?> UpdateAsync(int id, EventComponent ev, int userId);
        Task<bool> DeleteAsync(int id, int userId);
        Task<IEnumerable<EventDto>> GetAllForUserAsync(int userId);
        Task<EventDto?> GetByIdForUserAsync(int id, int userId);
    }
}
