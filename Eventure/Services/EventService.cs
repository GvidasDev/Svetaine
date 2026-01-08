using Eventure.Data;
using Eventure.Interfaces;
using Eventure.Models;
using Eventure.Dtos;
using Microsoft.EntityFrameworkCore;
using Eventure.Helpers;

namespace Eventure.Services
{
    public class EventService : IEventService
    {
        private readonly DatabaseContext _context;

        public EventService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var today = DateTime.UtcNow.Date;
            
            return await _context.Events
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    ImageUrl = e.ImageUrl,
                    InvitedUsers = e.InvitedUsers,
                    UserId = e.UserId,
                    Creator = _context.Users
                        .Where(u => u.Id == e.UserId)
                        .Select(u => u.Username)
                        .FirstOrDefault() ?? "Unknown",
                    RemainingDays = EventHelper.CalculateRemainingDays(e.Date)
                })
                .ToListAsync();
        }

        public async Task<EventDto?> GetByIdAsync(int id)
        {
            var today = DateTime.UtcNow.Date;
            var e = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (e == null) return null;

            var creator = await _context.Users
                .Where(u => u.Id == e.UserId)
                .Select(u => u.Username)
                .FirstOrDefaultAsync() ?? "Unknown";

            return new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                ImageUrl = e.ImageUrl,
                InvitedUsers = e.InvitedUsers,
                UserId = e.UserId,
                Creator = creator,
                RemainingDays = EventHelper.CalculateRemainingDays(e.Date)
            };
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
            if (existing == null) return null;

            existing.Title = ev.Title;
            existing.Description = ev.Description;
            existing.Date = ev.Date;
            existing.ImageUrl = ev.ImageUrl;
            existing.InvitedUsers = ev.InvitedUsers;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _context.Events.FindAsync(id);
            if (e == null) return false;

            _context.Events.Remove(e);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
