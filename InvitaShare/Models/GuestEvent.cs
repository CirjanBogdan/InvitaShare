namespace InvitaShare.Models
{
    public class GuestEvent
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int GuestId { get; set; }
        public GuestModel Guest { get; set; }
    }
}
