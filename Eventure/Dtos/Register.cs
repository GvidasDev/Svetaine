namespace Eventure.Dtos.Auth
{
    public class Register
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName  { get; set; } = "";
        public string Password  { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }
}
