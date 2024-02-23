using AutoMapper;
using InvitaShare.Data;
using InvitaShare.Models;
using InvitaShare.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvitaShare.Controllers
{
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GuestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            if (id != 0)
            {
                HttpContext.Session.SetInt32("currentEventId", id);
            }
            var currentEvent = await _context.Events.FindAsync(id);
            if (currentEvent != null)
            {
                ViewData["EventName"] = currentEvent.EventName;
            }
            IEnumerable<EventUserDTO> guestList = _context.EventUsers.Where(e => e.EventId == id).Select(b => new EventUserDTO
            {
                EventId = id,
                UserMail = b.ApplicationUser.Email,
            });
            return View(guestList);
        }

        public IActionResult CreateUserGuest(int id)
        {
            var capsuna = HttpContext.Session.GetInt32("currentEventId");
            if (id != 0)
            {
                HttpContext.Session.SetInt32("currentEventId", id);
            }
            
            var capsuna2 = HttpContext.Session.GetInt32("currentEventId");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserGuest(EventUserDTO guest)
        {
            var currentEventId = HttpContext.Session.GetInt32("currentEventId");
            try
            {
                if (ModelState.IsValid)
                {
                    var guestMail = guest.UserMail;
                    if (!string.IsNullOrEmpty(guestMail))
                    {
                        var guestUser = await _userManager.FindByEmailAsync(guestMail);
                        if (guestUser != null)
                        {
                            var guestUserId = _context.EventUsers.Where(e => e.EventId == currentEventId &&
                                              e.ApplicationUserId == guestUser.Id).Select(e => e.ApplicationUserId).FirstOrDefault();
                            if (guestUserId == null)
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
                                TempData["ViewMessage"] = "User has been invited.";
                                return RedirectToAction("Index", new { id = currentEventId });
                            }
                            TempData["ViewMessage"] = "The user has already been invited.";
                            return RedirectToAction("CreateUserGuest", new { id = currentEventId });
                        }
                    }
                }
                TempData["ViewMessage"] = "Username or Email does not exist. Please try again.";
                return RedirectToAction("CreateUserGuest", new { id = currentEventId });                
            } 
            catch (Exception ex)
            {
                TempData["ViewMessage"] = "The operation failed. Please try again." + ex.Message;
                return RedirectToAction("CreateUserGuest", new { id = currentEventId });
            }
        }
    }
}
