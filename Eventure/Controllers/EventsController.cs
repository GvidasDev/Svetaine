using System.Security.Claims;
using Eventure.Interfaces;
using Eventure.Models;
using Eventure.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _service;

        public EventsController(IEventService service)
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAll()
        {
            var list = await _service.GetAllAsync(CurrentUserId);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDto>> GetById(int id)
        {
            var ev = await _service.GetByIdAsync(id, CurrentUserId);
            if (ev == null) return NotFound();
            return Ok(ev);
        }

        [HttpPost]
        public async Task<ActionResult<EventComponent>> Create([FromBody] EventComponent ev)
        {
            ev.Id = 0;
            ev.UserId = CurrentUserId;

            var created = await _service.CreateAsync(ev);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<EventComponent>> Update(int id, [FromBody] EventComponent update)
        {
            update.Id = id;

            try
            {
                var updated = await _service.UpdateAsync(id, update, CurrentUserId);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch
            {
                return Forbid();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteAsync(id, CurrentUserId);
                if (!success) return NotFound();
                return NoContent();
            }
            catch
            {
                return Forbid();
            }
        }
    }
}
