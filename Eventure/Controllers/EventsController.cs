using Microsoft.AspNetCore.Mvc;
using Eventure.Models;
using Eventure.Interfaces;

namespace Eventure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;

    public EventsController(IEventService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await _service.GetByIdAsync(id);
        return ev == null ? NotFound() : Ok(ev);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EventComponent ev)
    {
        var created = await _service.CreateAsync(ev);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, EventComponent ev)
    {
        var updated = await _service.UpdateAsync(id, ev);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}