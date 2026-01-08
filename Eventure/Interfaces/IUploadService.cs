using Microsoft.AspNetCore.Http;

namespace Eventure.Interfaces
{
    public interface IUploadService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
