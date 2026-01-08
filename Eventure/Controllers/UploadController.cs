using Eventure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventure.Controllers
{
    [ApiController]
    [Route("api/upload")]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _service;

        public UploadController(IUploadService service)
        {
            _service = service;
        }

        [HttpPost("image")]
        [RequestSizeLimit(10_000_000)]
        public async Task<ActionResult> UploadImage([FromForm] IFormFile file)
        {
            try
            {
                string url = await _service.UploadImageAsync(file);
                return Ok(new { url = url });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
