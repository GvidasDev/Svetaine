using Eventure.Data;
using Eventure.Interfaces;
using Eventure.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventure.Services
{
    public class EventService : IEventService
    {
        private readonly DatabaseContext _context;

        public EventService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventComponent>> GetAllAsync()
        {
            return await _context.Events.AsNoTracking().ToListAsync();
        }

        public async Task<EventComponent?> GetByIdAsync(int id)
        {
            return await _context.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<EventComponent> CreateAsync(EventComponent ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return ev;
        }

        public async Task<EventComponent?> UpdateAsync(int id, EventComponent ev)
        {
            var existing = await _context.Events.FindAsync(id);
            if (existing == null)
                return null;

            existing.Title = ev.Title;
            existing.Description = ev.Description;
            existing.Date = ev.Date;
            existing.Creator = ev.Creator;
            existing.ImageUrl = ev.ImageUrl;
            existing.InvitedUsers = ev.InvitedUsers;
            existing.IsPublic = ev.IsPublic;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return false;

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
