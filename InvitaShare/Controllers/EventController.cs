using InvitaShare.Data;
using InvitaShare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace InvitaShare.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public int PageIndex { get; set; }

        public EventController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public IActionResult Test()
        {
            return View(_context.Events.ToList());
        }

        [Authorize]
        public async Task<IActionResult> Index(int? pageNumber, string? eventFilter)
        {
            var pageSize = 3;
            var events = _context.Events.AsQueryable();
            SetEventFilter(eventFilter, ref events);
            return View(await PaginatedList<Event>.CreateAsync(events, pageNumber ?? 1, pageSize));
        }

        public void SetEventFilter(string? eventFilter, ref IQueryable<Event> events)
        {
            if (eventFilter is not null)
                HttpContext.Session.SetString("eventFilter", eventFilter);
            else
                HttpContext.Session.SetString("eventFilter", "");

            eventFilter = HttpContext.Session.GetString("eventFilter") ?? string.Empty;
            if (!string.IsNullOrEmpty(eventFilter))
            {
                events = _context.Events.Where(e => e.EventType == eventFilter).AsQueryable();
                ViewData["eventFilterResult"] = eventFilter;
            }
        }

        [HttpPost]
        public IActionResult CreateNewEvent([Bind("EventType")] Event @event)
        {
            if (@event.EventType != null)
            {
                return RedirectToAction("Create" + @event.EventType.ToString() + "Event");
            }
            return RedirectToAction("Index");
        }
 
        public IActionResult CreateWeddingEvent()
        {
            return View();
        }

        public IActionResult CreateBaptismEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Hidden input for EventType in View
        public async Task<IActionResult> CreateEvent(Event eventModel)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            try
            {
                if (ModelState.IsValid)
                {
                    if (currentUser != null)
                    {
                        eventModel.CreatorUserId = currentUser.Id.ToString();
                        _context.Events.Add(eventModel);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else { return NotFound(); }
                }
            } catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
                return RedirectToAction("Create");
            }
            return RedirectToAction("Create");
        }

        

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateBaptismEvent(BaptismEvent baptismEvent)
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    if (ModelState.IsValid)
        //    {
        //        if (currentUser != null)
        //        {
        //            baptismEvent.CreatorUserId = currentUser.Id.ToString();
        //            baptismEvent.EventType = "Baptism".ToString();
        //            _context.BaptismEvents.Add(baptismEvent);
        //            _context.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        else { return NotFound(); }
        //    }
        //    return RedirectToAction("Create");
        //}
    }


}
