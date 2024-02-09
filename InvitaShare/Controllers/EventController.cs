using InvitaShare.Data;
using InvitaShare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index(string nextPage, string previousPage)
        {
            var resultPerPage = 3;
            var currentPage = HttpContext.Session.GetInt32("currentPage") ?? 0;
            var query = from s in _context.Events select s;
            var maxPage = Math.Ceiling(query.Count() / (double)resultPerPage - 1);
            if (!string.IsNullOrEmpty(nextPage) && maxPage > currentPage)
            {
                HttpContext.Session.SetInt32("currentPage", currentPage + 1);
            }
            else if (!string.IsNullOrEmpty(previousPage) && currentPage > 0)
            {
                HttpContext.Session.SetInt32("currentPage", currentPage - 1);
            } 
            else if (currentPage == maxPage || currentPage == 0)
            {
                HttpContext.Session.SetInt32("currentPage", currentPage);
            }
            else
            {
                HttpContext.Session.SetInt32("currentPage", 0);
            }
            currentPage = HttpContext.Session.GetInt32("currentPage") ?? 0;
            return View(await query.Skip(currentPage * 3).Take(resultPerPage).ToListAsync());
        }

        //[HttpPost]
        //public IActionResult CreateEvent(string EventType)
        //{
        //    if (EventType == "Wedding")
        //    {
        //        return RedirectToAction("CreateWeddingEvent");
        //    } else if (EventType == "Baptism")
        //    {
        //        return RedirectToAction("CreateBaptismEvent");
        //    }
        //    return RedirectToAction("Index"); 
        //}

        [HttpPost]
        public IActionResult CreateEvent([Bind("EventType")] Event @event)
        {
            if (@event.EventType != null)
            {
                return RedirectToAction("Create" + @event.EventType.ToString() + "Event");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult FilterEvents(string eventFilter)
        {
            if (eventFilter == "allUsers")
            {
                return RedirectToAction("Index");
            } else if (eventFilter == "wedding")
            {
                return RedirectToAction("WeddingFilter");
            } else if (eventFilter == "baptism")
            {
                return RedirectToAction("BaptismFilter");
            } else if (eventFilter == "myEvents")
            {
                return RedirectToAction("MyEventsFilter");
            }
            return NotFound();
        }

        public async Task<IActionResult> MyEventsFilter()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                IEnumerable<Event> events = _context.Events.Where(u => u.CreatorUserId == currentUser.Id).ToList();
                return View(events);
            }
            return View();
        }

        public IActionResult WeddingFilter()
        {
            IEnumerable<Event> events = _context.WeddingEvents;
            return View(events);
        }

        public IActionResult BaptismFilter()
        {
            IEnumerable<Event> events = _context.BaptismEvents;
            return View(events);
        }

        

        public IActionResult CreateWeddingEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWeddingEvent(WeddingEvent weddingEvent)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            try
            {
                if (ModelState.IsValid)
                {
                    if (currentUser != null)
                    {
                        weddingEvent.CreatorUserId = currentUser.Id.ToString();
                        weddingEvent.EventType = "Wedding".ToString();
                        _context.WeddingEvents.Add(weddingEvent);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else { return NotFound(); }
                }
            } catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return RedirectToAction("Create");
        }

        public IActionResult CreateBaptismEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBaptismEvent(BaptismEvent baptismEvent)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {
                    baptismEvent.CreatorUserId = currentUser.Id.ToString();
                    baptismEvent.EventType = "Baptism".ToString();
                    _context.BaptismEvents.Add(baptismEvent);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else { return NotFound(); }
            }
            return RedirectToAction("Create");
        }
    }


}
