using Eventure.Dtos.Invitations;

namespace Eventure.Interfaces
{
    public interface IInvitationService
    {
        Task<LookupUserResponseDto?> LookupByEmailAsync(string email);
        Task<List<InvitedUserDto>> GetInvitedAsync(int eventId, int currentUserId);
        Task AddAsync(int eventId, int currentUserId, string email);
        Task RemoveAsync(int eventId, int currentUserId, int userId);
    }
}
