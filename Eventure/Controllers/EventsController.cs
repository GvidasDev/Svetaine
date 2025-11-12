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

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAll()
        {
            var all = await _service.GetAllAsync();
            // tik prisijungusio vartotojo eventai
            var filtered = all.Where(e => e.UserId == CurrentUserId);
            return Ok(filtered);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDto>> GetById(int id)
        {
            var ev = await _service.GetByIdAsync(id);
            if (ev == null || ev.UserId != CurrentUserId)
                return NotFound();

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
            var existing = await _service.GetByIdAsync(id);
            if (existing == null || existing.UserId != CurrentUserId)
                return NotFound();

            update.UserId = CurrentUserId;
            var updated = await _service.UpdateAsync(id, update);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null || existing.UserId != CurrentUserId)
                return NotFound();

            var success = await _service.DeleteAsync(id);
            if (!success) return BadRequest();

            return NoContent();
        }
    }
}
