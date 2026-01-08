namespace Eventure.Models
{
    public class TaskState
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int UserId { get; set; }
        public int? EventId { get; set; }
    }
}
