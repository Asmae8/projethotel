using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeHotel.Models;

namespace SystemeHotel.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HotelDbContext _context;

        // 🔗 Injection du DbContext (connexion DB)
        public ClientsController(HotelDbContext context)
        {
            _context = context;
        }

        // 📄 GET: Clients
        public async Task<IActionResult> Index()
        {
            // SELECT * FROM Clients
            var clients = await _context.Clients.ToListAsync();
            return View(clients);
        }

        // ➕ GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // ✅ POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            // 🟢 INSERT INTO Clients
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ✏ GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

        // ✏ POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(client);
            }

            // 🟢 UPDATE Clients
            _context.Update(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ❌ GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null) return NotFound();

            return View(client);
        }

        // ❌ POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                // 🟢 DELETE FROM Clients
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
