using Eventure.Data;
using Eventure.Dtos.Account;
using Eventure.Interfaces;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Eventure.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext _db;

        public AccountService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<AccountDto> GetMeAsync(int userId)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            AccountDto dto = new AccountDto();
            dto.Id = user.Id;
            dto.Username = user.Username;
            dto.Email = user.Email;
            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;

            return dto;
        }

        public async Task<AccountDto> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            string email = (dto.Email ?? "").Trim();
            string firstName = (dto.FirstName ?? "").Trim();
            string lastName = (dto.LastName ?? "").Trim();

            bool emailTaken = await _db.Users.AnyAsync(u => u.Email == email && u.Id != userId);

            if (emailTaken)
            {
                throw new Exception("Email already in use.");
            }

            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;

            await _db.SaveChangesAsync();

            AccountDto result = new AccountDto();
            result.Id = user.Id;
            result.Username = user.Username;
            result.Email = user.Email;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;

            return result;
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                throw new Exception("Passwords do not match.");
            }

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            bool ok = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);

            if (!ok)
            {
                throw new Exception("Current password incorrect.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _db.SaveChangesAsync();
        }
    }
}
