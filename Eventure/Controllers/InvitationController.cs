using System.Security.Claims;
using Eventure.Dtos.Invitations;
using Eventure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _service;

        public InvitationController(IInvitationService service)
        {
            _service = service;
        }

        private int CurrentUserId
        {
            get
            {
                string? raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.Parse(raw!);
            }
        }

        [HttpGet("lookup")]
        public async Task<ActionResult> Lookup([FromQuery] string email)
        {
            var result = await _service.LookupByEmailAsync(email);
            return Ok(result);
        }

        [HttpGet("{eventId:int}")]
        public async Task<ActionResult> GetInvited(int eventId)
        {
            var list = await _service.GetInvitedAsync(eventId, CurrentUserId);
            return Ok(list);
        }

        [HttpPost("{eventId:int}")]
        public async Task<ActionResult> Add(int eventId, AddInviteDto dto)
        {
            await _service.AddAsync(eventId, CurrentUserId, dto.Email);
            return Ok();
        }

        [HttpDelete("{eventId:int}/{userId:int}")]
        public async Task<ActionResult> Remove(int eventId, int userId)
        {
            await _service.RemoveAsync(eventId, CurrentUserId, userId);
            return Ok();
        }
    }
}
