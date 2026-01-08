using Eventure.Data;
using Eventure.Dtos;
using Eventure.Helpers;
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

        public async Task<IEnumerable<EventDto>> GetAllAsync(int userId)
        {
            DateTime today = DateTime.UtcNow.Date;

            var query = _context.Events
                .Where(e => e.UserId == userId || _context.EventInvitations.Any(i => i.EventId == e.Id && i.UserId == userId));

            var list = await query
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    ImageUrl = e.ImageUrl,
                    UserId = e.UserId,
                    Creator = _context.Users
                        .Where(u => u.Id == e.UserId)
                        .Select(u => u.Username)
                        .FirstOrDefault() ?? "Unknown",
                    RemainingDays = EventHelper.CalculateRemainingDays(e.Date),
                    InvitedCount = _context.EventInvitations.Count(i => i.EventId == e.Id),
                    CanEdit = true,
                    CanInvite = e.UserId == userId
                })
                .ToListAsync();

            return list;
        }

        public async Task<EventDto?> GetByIdAsync(int id, int userId)
        {
            DateTime today = DateTime.UtcNow.Date;

            var ev = await _context.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (ev == null) return null;

            bool canAccess = await CanAccessAsync(id, userId);
            if (!canAccess) return null;

            string creator = await _context.Users
                .Where(u => u.Id == ev.UserId)
                .Select(u => u.Username)
                .FirstOrDefaultAsync() ?? "Unknown";

            int invitedCount = await _context.EventInvitations.CountAsync(i => i.EventId == id);

            EventDto dto = new EventDto();
            dto.Id = ev.Id;
            dto.Title = ev.Title;
            dto.Description = ev.Description;
            dto.Date = ev.Date;
            dto.ImageUrl = ev.ImageUrl;
            dto.UserId = ev.UserId;
            dto.Creator = creator;
            dto.RemainingDays = EventHelper.CalculateRemainingDays(ev.Date);
            dto.InvitedCount = invitedCount;
            dto.CanEdit = true;
            dto.CanInvite = ev.UserId == userId;

            return dto;
        }

        public async Task<EventComponent> CreateAsync(EventComponent ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return ev;
        }

        public async Task<EventComponent?> UpdateAsync(int id, EventComponent ev, int userId)
        {
            bool canAccess = await CanAccessAsync(id, userId);
            if (!canAccess) throw new Exception("Forbidden.");

            var existing = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return null;

            existing.Title = ev.Title;
            existing.Description = ev.Description;
            existing.Date = ev.Date;
            existing.ImageUrl = ev.ImageUrl;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var existing = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return false;

            if (existing.UserId != userId) throw new Exception("Forbidden.");

            var invitations = await _context.EventInvitations.Where(x => x.EventId == id).ToListAsync();
            if (invitations.Count > 0)
            {
                _context.EventInvitations.RemoveRange(invitations);
            }

            _context.Events.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> CanAccessAsync(int eventId, int userId)
        {
            var ev = await _context.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == eventId);
            if (ev == null) return false;

            if (ev.UserId == userId) return true;

            bool invited = await _context.EventInvitations.AnyAsync(i => i.EventId == eventId && i.UserId == userId);
            return invited;
        }
    }
}
