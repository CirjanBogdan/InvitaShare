using InvitaShare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InvitaShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index(string test)
        {
            var currentValue = HttpContext.Session.GetInt32("test") ?? 1;
            if (!string.IsNullOrEmpty(test))
            HttpContext.Session.SetInt32("test", currentValue + 1);

            ViewData["indexPage"] = HttpContext.Session.GetInt32("test");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}