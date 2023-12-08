using Microsoft.AspNetCore.Identity;

namespace InvitaShare.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<InvitedPerson> InvitedPersons { get; set; }
        public int TotalInvites { get; set; }
    }
}
