namespace InvitaShare.Models
{
    public class InvitedPerson
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Comments { get; set; }
        public bool RSVP { get; set; }
    }
}
