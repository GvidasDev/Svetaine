using Eventure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Eventure.Services
{
    public class UploadService : IUploadService
    {
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            string[] allowed = { "image/jpeg", "image/png", "image/webp", "image/gif" };
            bool okType = allowed.Contains(file.ContentType);

            if (!okType)
            {
                throw new Exception("Only image files are allowed.");
            }

            string uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            string ext = Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(ext))
            {
                ext = ".jpg";
            }

            string fileName = Guid.NewGuid().ToString("N") + ext;
            string fullPath = Path.Combine(uploadsDir, fileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // return URL path (frontend pridės hostą)
            return "/uploads/" + fileName;
        }
    }
}
