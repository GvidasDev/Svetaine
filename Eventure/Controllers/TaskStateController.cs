using System.Security.Claims;
using Eventure.Interfaces;
using Eventure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskStateController : ControllerBase
    {
        private readonly ITaskStateService _service;

        public TaskStateController(ITaskStateService service)
        {
            _service = service;
        }

        private int GetCurrentUserId()
        {
            string? raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(raw!);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            int userId = GetCurrentUserId();
            var states = await _service.GetAllAsync(userId);
            return Ok(states);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TaskState state)
        {
            int userId = GetCurrentUserId();
            var created = await _service.CreateAsync(state, userId);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, TaskState state)
        {
            int userId = GetCurrentUserId();
            var updated = await _service.UpdateAsync(id, state, userId);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            int userId = GetCurrentUserId();
            await _service.DeleteAsync(id, userId);
            return Ok();
        }
    }
}
