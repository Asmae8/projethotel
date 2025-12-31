using Microsoft.AspNetCore.Mvc;
using SystemeHotel.Models;
using System.IO;

namespace SystemeHotel.Controllers
{
    public class HomeController : Controller
    {
        private readonly HotelDbContext _context;

        public HomeController(HotelDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retourner la vue Razor pour que les tag helpers (asp-controller/asp-action) soient traités
            return View();
        }

        public IActionResult About()
        {
            // Retour explicite pour éviter les problèmes de casse sur certains systèmes de fichiers
            return View("~/Views/Home/about.cshtml");
        }

        public IActionResult Room()
        {
            return View("~/Views/Home/room.cshtml");
        }

        public IActionResult Gallery()
        {
            return View("~/Views/Home/gallery.cshtml");
        }

        public IActionResult Blog()
        {
            return View("~/Views/Home/blog.cshtml");
        }

        public IActionResult Contact()
        {
            return View("~/Views/Home/contact.cshtml");
        }
    }
}
