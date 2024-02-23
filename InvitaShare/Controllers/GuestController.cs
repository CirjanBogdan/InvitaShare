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

        public GuestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            if (TempData.ContainsKey("currentEventId"))
            {
                id = Convert.ToInt32(TempData["currentEventId"]);
            }
            if (id != 0)
            {
                TempData["currentEventId"] = id;
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

        public IActionResult CreateUserGuest()
        {
            if (TempData.ContainsKey("currentEventId"))
            {
                if (TempData["currentEventId"] is int eventId)
                {
                    TempData["currentEventId"] = eventId;
                    ViewBag.EventId = eventId;
                }
            }
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> CreateUserGuest(EventUserDTO guest)
        {
           try
            {
                var currentEventId = 0;
                if (TempData.ContainsKey("currentEventId"))
                {
                    currentEventId = Convert.ToInt32(TempData["currentEventId"]);
                }
                if (ModelState.IsValid)
                {
                    var guestMail = guest.UserMail;
                    if (guestMail != null)
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
                            }
                            else
                            {
                                TempData["ErrorMes"] = "User already invited.";
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
                TempData["currentEventId"] = currentEventId;
                return RedirectToAction("Index");
            } catch (Exception ex)
            {
                TempData["ErrorMes"] = "The operation failed. Please try again." + ex.Message;
                return RedirectToAction("CreateGuestRegistered");
            }
        }
    }
}
