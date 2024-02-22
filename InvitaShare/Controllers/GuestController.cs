using AutoMapper;
using InvitaShare.Data;
using InvitaShare.Models;
using InvitaShare.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvitaShare.Controllers
{
    // Sessions are not saved at RedirectToAction
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public int PageIndex { get; set; }

        public GuestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int id)
        {
            var capsuna = HttpContext.Session.GetInt32("currentEventId") ?? 0;
            if (id != 0)
            {
                HttpContext.Session.SetInt32("currentEventId", id);
            }
            var currentEventId = HttpContext.Session.GetInt32("currentEventId") ?? 0;
            var currentEvent = await _context.Events.FindAsync(currentEventId);
            if (currentEvent != null)
            {
                ViewData["EventName"] = currentEvent.EventName;
                ViewBag.EventId = currentEventId;
            }
            IEnumerable<EventUserDTO> eventUserDTOs = _context.EventUsers.Where(e => e.EventId == currentEventId).Select(b => new EventUserDTO
            {
                UserMail = b.ApplicationUser.Email,
            });
            return View(eventUserDTOs);
        }

        public IActionResult CreateGuestRegistered()
        {
            var id = HttpContext.Session.GetInt32("currentEventId") ?? 0;
            ViewBag.EventId = id;
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> CreateGuestRegistered(EventUserDTO guest)
        {
            var currentEventId = HttpContext.Session.GetInt32("currentEventId") ?? 0;
            if (ModelState.IsValid)
            {
                var guestMail = guest.UserMail;
                if (guestMail != null)
                {
                    
                    var guestUser = await _userManager.FindByEmailAsync(guestMail);
                    if (guestUser != null)
                    {
                        var existedUser = _context.EventUsers.Where(e => e.EventId == currentEventId &&
                                          e.ApplicationUserId == guestUser.Id).Select(e => e.ApplicationUserId).FirstOrDefault();
                        if (existedUser == null)
                        {
                            EventUser eventUser = new EventUser()
                            {
                                EventId = currentEventId,
                                ApplicationUserId = guestUser.Id
                            };
                            guestUser.Invites++;
                            await _userManager.UpdateAsync(guestUser);
                            _context.EventUsers.Add(eventUser);
                            await _context.SaveChangesAsync();
                        } else
                        {
                            TempData["ErrorMes"] = "User allready invited.";
                            return RedirectToAction("CreateGuestRegistered");
                        }
                    }
                    else
                    {
                        TempData["ErrorMes"] = "Username or Email does not exist. Please try again.";
                        return RedirectToAction("CreateGuestRegistered");
                    }
                }
            }
            else
            {
                TempData["ErrorMes"] = "Username or Email does not exist. Please try again.";
                return RedirectToAction("CreateGuestRegistered");
            }
            return RedirectToAction("Index", new { id = currentEventId });
        }
    }
}
