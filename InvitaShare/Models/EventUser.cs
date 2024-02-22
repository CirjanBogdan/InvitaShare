using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvitaShare.Models
{
    public class EventUser
    {
        
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int? EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }


    }
}
