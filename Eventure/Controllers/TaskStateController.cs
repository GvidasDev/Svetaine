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

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var states = await _service.GetAllAsync();
            return Ok(states);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TaskState state)
        {
            var created = await _service.CreateAsync(state);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TaskState state)
        {
            var updated = await _service.UpdateAsync(id, state);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
