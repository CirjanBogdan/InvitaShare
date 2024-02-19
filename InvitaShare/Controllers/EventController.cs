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
        public int PageIndex { get; set; }

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
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes.");
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
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes.");
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
                var oldWedding = _context.WeddingEvents.FirstOrDefault(s => s.Id == weddingEvent.Id);
                if (oldWedding != null)
                {
                    _mapper.Map(weddingEvent, oldWedding);
                    _context.Update(oldWedding);
                    await _context.SaveChangesAsync();
                }
                else
                {
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
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return View("EditBaptismEvent", baptismDTO);
            }
            return RedirectToAction("Index");
        }
    }
}
