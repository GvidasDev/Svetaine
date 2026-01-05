using Eventure.Dtos.Account;

namespace Eventure.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto> GetMeAsync(int userId);
        Task<AccountDto> UpdateProfileAsync(int userId, UpdateProfileDto dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }
}
