namespace InvitaShare.Models
{
    public class GuestViewModel : Guest
    {
        public ICollection<Guest> Guests { get; set; }
        public Guest guest { get; set; }
    }
}
