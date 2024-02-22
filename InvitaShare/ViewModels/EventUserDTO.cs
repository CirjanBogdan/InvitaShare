using System.ComponentModel.DataAnnotations;

namespace InvitaShare.ViewModels
{
    public class EventUserDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? UserMail { get; set; }
    }
}
