using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SystemeHotel;
using SystemeHotel.Models;

namespace SystemeHotel.Controllers
{
    public class FacturesController : Controller
    {
        private readonly HotelDbContext _context;

        public FacturesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Factures
        public async Task<IActionResult> Index()
        {
            var factures = _context.Factures
                .Include(f => f.Reservation)
                .Include(f => f.Paiement);
            return View(await factures.ToListAsync());
        }

        // GET: Factures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var facture = await _context.Factures
                .Include(f => f.Reservation)
                .Include(f => f.Paiement)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (facture == null) return NotFound();

            return View(facture);
        }

        // GET: Factures/Create
        public IActionResult Create()
        {
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id");
            return View();
        }

        // POST: Factures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,Statut,DateEmission,ReservationId")] Facture facture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", facture.ReservationId);
            return View(facture);
        }

        // GET: Factures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var facture = await _context.Factures.FindAsync(id);
            if (facture == null) return NotFound();

            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", facture.ReservationId);
            return View(facture);
        }

        // POST: Factures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Total,Statut,DateEmission,ReservationId")] Facture facture)
        {
            if (id != facture.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactureExists(facture.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", facture.ReservationId);
            return View(facture);
        }

        // GET: Factures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var facture = await _context.Factures
                .Include(f => f.Reservation)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (facture == null) return NotFound();

            return View(facture);
        }

        // POST: Factures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facture = await _context.Factures.FindAsync(id);
            if (facture != null)
            {
                _context.Factures.Remove(facture);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FactureExists(int id)
        {
            return _context.Factures.Any(f => f.Id == id);
        }

        // GET: Factures/Pay/5
        public async Task<IActionResult> Pay(int? id)
        {
            if (id == null) return NotFound();

            var facture = await _context.Factures
                .Include(f => f.Reservation)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (facture == null) return NotFound();

            return View(facture);
        }

        // POST: Factures/Pay/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int id, string numeroCarte, string dateExpiration, string cvv, string nomTitulaire)
        {
            var facture = await _context.Factures.FindAsync(id);
            if (facture == null) return NotFound();

            // Validation simple des informations de paiement
            if (string.IsNullOrEmpty(numeroCarte) || numeroCarte.Length != 16 ||
                string.IsNullOrEmpty(dateExpiration) || string.IsNullOrEmpty(cvv) || cvv.Length != 3 ||
                string.IsNullOrEmpty(nomTitulaire))
            {
                ModelState.AddModelError("", "Informations de paiement invalides.");
                return View(facture);
            }

            // Vérifier si la facture a déjà un paiement
            if (_context.Paiements.Any(p => p.FactureId == facture.Id))
            {
                ModelState.AddModelError("", "Cette facture a déjà été payée.");
                return View(facture);
            }

            var paiement = new PaiementEnLigne
            {
                Montant = facture.Total,
                Mode = ModePaiement.EnLigne,
                FactureId = facture.Id,
                Facture = facture,
                TransactionId = Guid.NewGuid().ToString()
            };

            _context.Paiements.Add(paiement);
            facture.Statut = "Payée";

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = facture.Id });
        }
    }
}
