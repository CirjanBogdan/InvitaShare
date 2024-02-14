namespace InvitaShare.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public string? GuestUserName { get; set; }
        public string? GuestEmail { get; set; }
    }
}
