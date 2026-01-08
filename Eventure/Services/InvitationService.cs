using Eventure.Data;
using Eventure.Dtos.Invitations;
using Eventure.Interfaces;
using Eventure.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventure.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly DatabaseContext _db;

        public InvitationService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<LookupUserResponseDto?> LookupByEmailAsync(string email)
        {
            string e = (email ?? "").Trim().ToLower();
            if (string.IsNullOrWhiteSpace(e)) return null;

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToLower() == e);
            if (user == null) return null;

            LookupUserResponseDto dto = new LookupUserResponseDto();
            dto.Id = user.Id;
            dto.Username = user.Username;
            dto.Email = user.Email;

            return dto;
        }

        public async Task<List<InvitedUserDto>> GetInvitedAsync(int eventId, int currentUserId)
        {
            await EnsureCanAccessEventAsync(eventId, currentUserId);

            return await _db.EventInvitations
                .Where(x => x.EventId == eventId)
                .Select(x => new InvitedUserDto
                {
                    Id = x.User.Id,
                    Username = x.User.Username,
                    Email = x.User.Email
                })
                .OrderBy(x => x.Username)
                .ToListAsync();
        }

        public async Task AddAsync(int eventId, int currentUserId, string email)
        {
            await EnsureIsCreatorAsync(eventId, currentUserId);

            string e = (email ?? "").Trim().ToLower();
            if (string.IsNullOrWhiteSpace(e)) throw new Exception("Email required.");

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == e);
            if (user == null) throw new Exception("User not found.");

            var ev = await _db.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == eventId);
            if (ev == null) throw new Exception("Event not found.");

            if (user.Id == ev.UserId) throw new Exception("Creator cannot be invited.");

            bool exists = await _db.EventInvitations.AnyAsync(x => x.EventId == eventId && x.UserId == user.Id);
            if (exists) return;

            EventInvitation inv = new EventInvitation();
            inv.EventId = eventId;
            inv.UserId = user.Id;

            _db.EventInvitations.Add(inv);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(int eventId, int currentUserId, int userId)
        {
            await EnsureIsCreatorAsync(eventId, currentUserId);

            var inv = await _db.EventInvitations.FirstOrDefaultAsync(x => x.EventId == eventId && x.UserId == userId);
            if (inv == null) return;

            _db.EventInvitations.Remove(inv);
            await _db.SaveChangesAsync();
        }

        private async Task EnsureCanAccessEventAsync(int eventId, int currentUserId)
        {
            var ev = await _db.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == eventId);
            if (ev == null) throw new Exception("Event not found.");

            if (ev.UserId == currentUserId) return;

            bool invited = await _db.EventInvitations.AnyAsync(x => x.EventId == eventId && x.UserId == currentUserId);
            if (!invited) throw new Exception("Forbidden.");
        }

        private async Task EnsureIsCreatorAsync(int eventId, int currentUserId)
        {
            var ev = await _db.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == eventId);
            if (ev == null) throw new Exception("Event not found.");

            if (ev.UserId != currentUserId) throw new Exception("Forbidden.");
        }
    }
}
