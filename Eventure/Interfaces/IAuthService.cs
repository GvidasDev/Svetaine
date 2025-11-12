using Eventure.Dtos.Auth;

namespace Eventure.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(Register req);
        Task<AuthResponse> LoginAsync(Login req);
        Task<bool> VerifyTokenAsync(int userId);
    }
}
