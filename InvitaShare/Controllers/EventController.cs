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

        public EventController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
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

        public IActionResult ShowAllEvents()
        {
            IEnumerable<Event> events = _context.Events;
            return View(events); 
        }


        public IActionResult Create()
        {
            return View();
        }


        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(string EventType)
        {
            if (EventType == "Wedding")
            {
                return RedirectToAction("CreateWeddingEvent");
            } else if (EventType == "Baptism")
            {
                return RedirectToAction("CreateBaptismEvent");
            }
            return RedirectToAction("Index"); //tre sa ma intorc daca e alt tip in afara de wedding si baptism
        }








        //public async Task<IActionResult> CreateEvent(Event newEvent)
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    if (ModelState.IsValid)
        //    {
        //        if (currentUser != null)
        //        {
        //            weddingEvent.CreatorUserId = currentUser.Id.ToString();
        //            weddingEvent.EventType = "Wedding".ToString();
        //            _context.WeddingEvents.Add(weddingEvent);
        //            _context.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        else { return NotFound(); }
        //    }
        //    return RedirectToAction("Create");
        //}

        public IActionResult CreateWeddingEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWeddingEvent(WeddingEvent weddingEvent)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {
                    weddingEvent.CreatorUserId = currentUser.Id.ToString();
                    weddingEvent.EventType = "Wedding".ToString();
                    _context.WeddingEvents.Add(weddingEvent);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                } else { return NotFound(); }                
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
