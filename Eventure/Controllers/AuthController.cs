using BCrypt.Net;
using Eventure.Data;
using Eventure.Dtos.Auth;
using Eventure.Helpers;
using Eventure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Eventure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly JwtTokenGenerator _jwt;

        public AuthController(DatabaseContext db, JwtTokenGenerator jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(Register req)
        {
            if (req.Password != req.ConfirmPassword)
                return BadRequest("Passwords do not match.");

            if (await _db.Users.AnyAsync(u => u.Username == req.Username))
                return BadRequest("Username already taken.");

            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                return BadRequest("Email already in use.");

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

            var token = _jwt.Generate(user);
            return Ok(new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(Login req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
            if (user is null) return Unauthorized("Invalid credentials.");

            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _jwt.Generate(user);
            return Ok(new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email
            });
        }

        [HttpGet("verify")]
        [Authorize]
        public IActionResult VerifyToken()
        {   
            return Ok(new { message = "Token valid" });
        }
    }
}
