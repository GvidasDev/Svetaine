using System.ComponentModel.DataAnnotations;

namespace Eventure.Models
{
    public class EventExpense
    {
        [Key]
        public int Id { get; set; }

        public int EventId { get; set; }
        public int UserId { get; set; }

        public decimal Amount { get; set; }
        public string Note { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
