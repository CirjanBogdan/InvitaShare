using InvitaShare.Data;
using Microsoft.AspNetCore.Identity;

namespace InvitaShare
{
    public class PaginatedList<T> : List<T>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public int NumarPagina { get; set; }

        public PaginatedList(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        // vreau sa folosesc "NumarPagina" ca sa pot pasa data din controller si view si invers/// si Modelul de Event in Index
        
    }
}
