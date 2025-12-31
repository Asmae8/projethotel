using Microsoft.AspNetCore.Mvc;
using SystemeHotel.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SystemeHotel.Controllers
{
    public class AccountController : Controller
    {
        // private readonly HotelDbContext _context;

        // public AccountController(HotelDbContext context)
        // {
        //     _context = context;
        // }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string role)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                ModelState.AddModelError("", "Tous les champs sont requis.");
                return View();
            }

            bool isValid = false;
            string userName = "";
            string userRole = role;

            if (role == "Client")
            {
                // Pour la démo, accepter n'importe quel email comme client
                isValid = true;
                userName = email.Split('@')[0]; // Utiliser la partie avant @ comme nom
            }
            else if (role == "Admin")
            {
                // Pour la démo, admin avec email "admin@hotel.com" et mot de passe "admin"
                if (email == "admin@hotel.com" && password == "admin")
                {
                    isValid = true;
                    userName = "Administrateur";
                }
            }

            if (isValid)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, userRole)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email, mot de passe ou rôle incorrect.");
            return View();
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}