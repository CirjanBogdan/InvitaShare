using InvitaShare.Data;
using InvitaShare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvitaShare.Controllers
{
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GuestController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Event currentEvent = new Event();

        public IActionResult Index(int id)
        {
            ICollection<Guest> invites = _context.Guests.Where(e => e.EventId == id).ToList();
            Guest newGuest = new Guest();
            GuestViewModel vm = new GuestViewModel
            {
                Guests = invites,
                guest = newGuest,
            };
            currentEvent.Id = id;
            return View(vm);
            // tre sa trimit un viewModel in loc de invites boss-u meu ca sa mearga
        }

        //ma gandeam sa folosesc 2 modele in view si sa am butonul de creeaza guest direct in Index, practic sa apelez direct din Index post create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guest guest)
        {
            Event ev = new Event();
            ev = _context.Events.Where(e => e.Id == currentEvent.Id).FirstOrDefault();
            ev.Guests.Add(guest);
            _context.Events.Update(ev);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
