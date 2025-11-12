using BCrypt.Net;
using Eventure.Data;
using Eventure.Dtos.Auth;
using Eventure.Helpers;
using Eventure.Interfaces;
using Eventure.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventure.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _db;
        private readonly JwtTokenGenerator _jwt;

        public AuthService(DatabaseContext db, JwtTokenGenerator jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        public async Task<AuthResponse> RegisterAsync(Register req)
        {
            if (req.Password != req.ConfirmPassword)
                throw new Exception("Passwords do not match.");

            if (await _db.Users.AnyAsync(u => u.Username == req.Username))
                throw new Exception("Username already taken.");

            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                throw new Exception("Email already in use.");

            var user = new User
            {
                Username = req.Username,
                Email = req.Email,
                FirstName = req.FirstName,
                LastName = req.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new AuthResponse
            {
                Token = _jwt.Generate(user),
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<AuthResponse> LoginAsync(Login req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
            if (user is null)
                throw new Exception("Invalid credentials.");

            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                throw new Exception("Invalid credentials.");

            return new AuthResponse
            {
                Token = _jwt.Generate(user),
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public Task<bool> VerifyTokenAsync(int userId)
        {
            return Task.FromResult(true);
        }
    }
}
