using System.Security.Claims;
using Eventure.Dtos.Expenses;
using Eventure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventure.Controllers
{
    [ApiController]
    [Route("api/events/{eventId:int}/expenses")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _service;

        public ExpensesController(IExpenseService service)
        {
            _service = service;
        }

        private int GetCurrentUserId()
        {
            string? raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(raw!);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(int eventId)
        {
            int userId = GetCurrentUserId();
            var list = await _service.GetExpensesAsync(eventId, userId);
            return Ok(list);
        }

        [HttpGet("balances")]
        public async Task<ActionResult> GetBalances(int eventId)
        {
            int userId = GetCurrentUserId();
            var list = await _service.GetBalancesAsync(eventId, userId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Create(int eventId, CreateExpenseDto dto)
        {
            int userId = GetCurrentUserId();
            var created = await _service.AddExpenseAsync(eventId, userId, dto);
            return Ok(created);
        }
    }
}
