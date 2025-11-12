namespace Eventure.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? ImageUrl { get; set; }
        public string? InvitedUsers { get; set; }
        public bool IsPublic { get; set; }
        public int UserId { get; set; }
        public string Creator { get; set; } = "Unknown";
        public int RemainingDays { get; set; }
    }
}
