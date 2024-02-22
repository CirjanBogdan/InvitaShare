using Microsoft.AspNetCore.Identity;

namespace InvitaShare.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Invites { get; set; }
        public ICollection<Event> Events { get; } = new List<Event>();
        public ICollection<EventUser> EventUsers { get; set; }
    }
}
