namespace Eventure.Models
{
    public class EventInvitation
    {
        public int EventId { get; set; }
        public EventComponent Event { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
