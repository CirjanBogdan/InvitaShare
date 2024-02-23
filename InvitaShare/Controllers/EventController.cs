using InvitaShare.Data;
using InvitaShare.Models;
using InvitaShare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace InvitaShare.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public EventController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> Index(int? pageNumber, string? eventFilter)
        {
            var pageSize = 3;
            var events =  _context.Events.AsQueryable();
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
        public IActionResult CreateNewEvent([Bind("EventType")] Event newEvent)
        {
            if (newEvent.EventType != null)
            {
                return RedirectToAction("Create" + newEvent.EventType.ToString() + "Event");
            }
            return RedirectToAction("Index");
        }

        public IActionResult CreateWeddingEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWeddingEvent(WeddingEventDTO weddingModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        var _wedding = _mapper.Map<WeddingEvent>(weddingModel);
                        _wedding.ApplicationUserId = user.Id;
                        _context.WeddingEvents.Add(_wedding);
                        await _context.SaveChangesAsync();
                        TempData["ViewMessage"] = "The event has been successfuly created.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                TempData["ViewMessage"] = "The operation failed. Please try again." + ex.Message;
            }
            return RedirectToAction("Index");
        }

        public IActionResult CreateBaptismEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBaptismEvent(BaptismEventDTO baptismModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        var _baptism = _mapper.Map<BaptismEvent>(baptismModel);
                        _baptism.ApplicationUserId = user.Id;
                        _context.BaptismEvents.Add(_baptism);
                        await _context.SaveChangesAsync();
                        TempData["ViewMessage"] = "The event has been successfuly created.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                TempData["ViewMessage"] = "The operation failed. Please try again." + ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("EditEvent/{id}")]
        public IActionResult EditEvent(int id)
        {
            var ev = _context.Events.FirstOrDefault(s => s.Id == id);

            if (ev == null)
            {
                return NotFound(); 
            }
            if (ev is WeddingEvent)
            {
                return View("EditWeddingEvent", ev);
            }
            else if (ev is BaptismEvent)
            {
                return View("EditBaptismEvent", ev);
            }
            return BadRequest("Invalid event type");
        }

        [HttpPost]
        public async Task<IActionResult> EditWeddingEvent(WeddingEventDTO weddingEvent)
        {
            if (ModelState.IsValid)
            {
                var currentWeddingEvent = _context.WeddingEvents.FirstOrDefault(s => s.Id == weddingEvent.Id);
                if (currentWeddingEvent != null)
                {
                    _mapper.Map(weddingEvent, currentWeddingEvent);
                    _context.Update(currentWeddingEvent);
                    await _context.SaveChangesAsync();
                    TempData["ViewMessage"] = "The event has been successfuly Updated.";
                }
                else
                {
                    TempData["ViewMessage"] = "The operation failed. Please try again.";
                    return NotFound(); 
                }
            }
            else
            {
                return View("EditWeddingEvent", weddingEvent);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditBaptismEvent(BaptismEventDTO baptismDTO)
        {
            if (ModelState.IsValid)
            {
                var baptism = _context.BaptismEvents.FirstOrDefault(s => s.Id == baptismDTO.Id);
                if (baptism != null)
                {
                    _mapper.Map(baptismDTO, baptism);
                    _context.Update(baptism);
                    await _context.SaveChangesAsync();
                    TempData["ViewMessage"] = "The event has been successfuly Updated.";
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                TempData["ViewMessage"] = "The operation failed. Please try again.";
                return View("EditBaptismEvent", baptismDTO);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("DetailsEvent/{id}")]
        public IActionResult DetailsEvent(int id)
        {
            var currentEvent = _context.Events.SingleOrDefault(s => s.Id == id);

            if (currentEvent == null)
            {
                return NotFound();
            }
            if (currentEvent is WeddingEvent weddingEvent)
            {
                return View("DetailsWeddingEvent", weddingEvent);
            }
            else if (currentEvent is BaptismEvent baptismEvent)
            {
                return View("DetailsBaptismEvent", baptismEvent);
            }
            return BadRequest("Invalid event type");
        }

        public IActionResult DetailsWeddingEvent(int id)
        {
            var weddingEvent = _context.Events.OfType<WeddingEvent>().SingleOrDefault(i => i.Id == id);
            if (weddingEvent == null)
            {
                return NotFound();
            }
            return View(weddingEvent);
        }

        public IActionResult DetailsBaptismEvent(int id)
        {
            var baptismEvent = _context.Events.OfType<BaptismEvent>().SingleOrDefault(i => i.Id == id);
            if (baptismEvent == null)
            {
                return NotFound();
            }
            return View(baptismEvent);
        }

        [HttpGet]
        [Route("DeleteEvent/{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var even = _context.Events.SingleOrDefault(s => s.Id == id);

            if (even == null)
            {
                return NotFound();
            }
            if (even is WeddingEvent weddingEvent)
            {
                return View("DeleteWeddingEvent", weddingEvent);
            }
            else if (even is BaptismEvent baptismEvent)
            {
                return View("DeleteBaptismEvent", baptismEvent);
            }
            return BadRequest("Invalid event type");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWeddingEvent(int id)
        {
            var weddingEvent = await _context.WeddingEvents.FindAsync(id);
            if (weddingEvent == null)
            {
                return NotFound();
            }
            _context.WeddingEvents.Remove(weddingEvent);
            await _context.SaveChangesAsync();
            TempData["ViewMessage"] = "The event has been successfuly deleted.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBaptismEvent(int id)
        {
            var baptismEvent = await _context.BaptismEvents.FindAsync(id);
            if (baptismEvent == null)
            {
                return NotFound();
            }
            _context.BaptismEvents.Remove(baptismEvent);
            await _context.SaveChangesAsync();
            TempData["ViewMessage"] = "The event has been successfuly deleted.";
            return RedirectToAction("Index");
        }

    }
}
