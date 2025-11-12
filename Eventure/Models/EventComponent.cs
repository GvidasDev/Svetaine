using System.ComponentModel.DataAnnotations;
using Eventure.Helpers;

namespace Eventure.Models
{
    public class EventComponent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Pavadinimas privalomas")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Data privaloma")]
        public DateTime Date { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [MaxLength(100)]
        public string Creator { get; set; } = string.Empty;

        public string? InvitedUsers { get; set; }

        public bool IsPublic { get; set; } = true;

        public int RemainingDays => EventHelper.CalculateRemainingDays(Date);
    }
}
