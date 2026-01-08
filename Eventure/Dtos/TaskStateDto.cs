namespace Eventure.Dtos
{
    public class TaskStateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int? EventId { get; set; }
        public string? EventTitle { get; set; }
    }
}
