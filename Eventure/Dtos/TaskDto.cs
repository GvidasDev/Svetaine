namespace Eventure.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TaskStatus { get; set; }
        public int? EventId { get; set; }
        public int UserId { get; set; }
        public string? EventTitle { get; set; }
    }
}
