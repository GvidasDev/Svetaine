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
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<ActionResult> GetMyTasks()
        {
            int userId = CurrentUserId;
            var tasks = await _service.GetAllAsync(userId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TaskDto dto)
        {
            int userId = CurrentUserId;

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                TaskStatus = dto.TaskStatus,
                EventId = dto.EventId,
                UserId = userId
            };

            var result = await _service.CreateAsync(task);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TaskDto dto)
        {
            int userId = CurrentUserId;
            var updated = await _service.UpdateAsync(id, new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                TaskStatus = dto.TaskStatus,
                EventId = dto.EventId,
                UserId = userId
            }, userId);

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            int userId = CurrentUserId;
            await _service.DeleteAsync(id, userId);
            return Ok();
        }
    }
}
