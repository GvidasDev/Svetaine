namespace Eventure.Dtos.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = "";
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
