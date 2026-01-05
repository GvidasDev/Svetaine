using System.Security.Claims;
using Eventure.Dtos.Account;
using Eventure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            userId = 0;

            string? raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(raw))
            {
                return false;
            }

            return int.TryParse(raw, out userId);
        }

        [HttpGet("me")]
        public async Task<ActionResult> GetMe()
        {
            int userId;

            if (!TryGetCurrentUserId(out userId))
            {
                return Unauthorized("Missing user id claim.");
            }

            AccountDto dto = await _service.GetMeAsync(userId);
            return Ok(dto);
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            int userId;

            if (!TryGetCurrentUserId(out userId))
            {
                return Unauthorized("Missing user id claim.");
            }

            AccountDto updated = await _service.UpdateProfileAsync(userId, dto);
            return Ok(updated);
        }

        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            int userId;

            if (!TryGetCurrentUserId(out userId))
            {
                return Unauthorized("Missing user id claim.");
            }

            await _service.ChangePasswordAsync(userId, dto);
            return Ok();
        }
    }
}
