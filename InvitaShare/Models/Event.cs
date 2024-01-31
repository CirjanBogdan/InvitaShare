using System.ComponentModel.DataAnnotations;

namespace InvitaShare.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? CreatorUserId { get; set; }
        public string? PartnerName1 { get; set; }
        public string? PartnerName2 { get; set; }
        public string? ParentName1 { get; set; }
        public string? ParentName2 { get; set; }
        public string? EventType { get; set; }
        public string? RestaurantName { get; set; }
        public string? ChurchName { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public ICollection<Guest> Guests { get; set; } = new List<Guest>();
        public string CreatedOnView
        {
            get { return CreatedOn.ToShortDateString(); }
        }
    }
}
