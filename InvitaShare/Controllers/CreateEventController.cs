using InvitaShare.Data;
using InvitaShare.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvitaShare.Controllers
{
    public class CreateEventController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CreateEventController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<InvitedPerson> guestList = _db.InvitedPersons.ToList();
            return View(guestList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(InvitedPerson guest)
        {
            if (ModelState.IsValid)
            {
                _db.InvitedPersons.Add(guest);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var guest = _db.InvitedPersons.Find(id);
            return View(guest);
        }

        [HttpPost]
        public IActionResult Edit(InvitedPerson invitedPerson)
        {
            if (ModelState.IsValid)
            {
                _db.InvitedPersons.Update(invitedPerson);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invitedPerson);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var guest = _db.InvitedPersons.Find(id);
            return View(guest);
        }

        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var guest = _db.InvitedPersons.Find(id);
            if (ModelState.IsValid)
            {
                if (guest is not null)
                {
                    _db.InvitedPersons.Remove(guest);
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(guest);
        }
    }
}
