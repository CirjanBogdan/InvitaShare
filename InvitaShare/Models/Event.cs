using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvitaShare.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string EventName { get; set; } = null!;
        [Required]
        public string EventType { get; set; } = null!;
        public string? RestaurantName { get; set; }
        public string? ChurchName { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedOnView
        {
            get { return CreatedOn.ToShortDateString(); }
        }
        public string ApplicationUserId { get; set; } = null!;
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
