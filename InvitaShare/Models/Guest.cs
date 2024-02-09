using System.ComponentModel.DataAnnotations;

namespace InvitaShare.Models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? FullName { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
        
    }
}
