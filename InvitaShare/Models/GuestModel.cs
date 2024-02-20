namespace InvitaShare.Models
{
    public class GuestModel : ApplicationUser
    {
        public int GuestId { get; set; } 
        public string? GuestName { get; set; }
    }
}
