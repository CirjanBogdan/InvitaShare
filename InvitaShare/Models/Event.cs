using System.ComponentModel.DataAnnotations;

namespace InvitaShare.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string EventName { get; set; } = null!;
        public string? CreatorUserId { get; set; }
        public string? EventType { get; set; }
        public string? RestaurantName { get; set; }
        public string? ChurchName { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedOnView
        {
            get { return CreatedOn.ToShortDateString(); }
        }
    }
}
