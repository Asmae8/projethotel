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
    public class ReservationsController : Controller
    {
        private readonly HotelDbContext _context;

        public ReservationsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var hotelDbContext = _context.Reservations
                .Include(r => r.Chambre)
                .Include(r => r.Client);

            return View(await hotelDbContext.ToListAsync());
        }

        // GET: Reservations/Create
        public IActionResult Create(DateTime? DateDebut, DateTime? DateFin)
        {
            ViewBag.ChambreId = new SelectList(_context.Chambres, "Id", "Numero");
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom");

            // Pré-remplir le modèle avec les dates si fournies
            var reservation = new Reservation();
            if (DateDebut.HasValue)
                reservation.DateDebut = DateDebut.Value;
            if (DateFin.HasValue)
                reservation.DateFin = DateFin.Value;

            return View(reservation);
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("DateDebut,DateFin,ClientId,ChambreId")] Reservation reservation)
        {
            // Vérifier la validité du modèle
            if (!ModelState.IsValid)
            {
                ViewBag.ChambreId = new SelectList(_context.Chambres, "Id", "Numero", reservation.ChambreId);
                ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", reservation.ClientId);
                return View(reservation);
            }

            // Validation personnalisée : DateFin > DateDebut
            if (reservation.DateFin <= reservation.DateDebut)
            {
                ModelState.AddModelError("", "La date de départ doit être après la date d'arrivée.");
                ViewBag.ChambreId = new SelectList(_context.Chambres, "Id", "Numero", reservation.ChambreId);
                ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", reservation.ClientId);
                return View(reservation);
            }

            // Vérifier que la chambre existe
            var chambre = await _context.Chambres.FindAsync(reservation.ChambreId);
            if (chambre == null)
            {
                ModelState.AddModelError("", "Chambre introuvable. Sélectionnez une chambre valide.");
                ViewBag.ChambreId = new SelectList(_context.Chambres, "Id", "Numero", reservation.ChambreId);
                ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", reservation.ClientId);
                return View(reservation);
            }

            // Vérifier que le client existe
            var client = await _context.Clients.FindAsync(reservation.ClientId);
            if (client == null)
            {
                ModelState.AddModelError("", "Client introuvable. Sélectionnez un client valide.");
                ViewBag.ChambreId = new SelectList(_context.Chambres, "Id", "Numero", reservation.ChambreId);
                ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", reservation.ClientId);
                return View(reservation);
            }

            // Vérifier la disponibilité de la chambre
            if (!chambre.EstDisponible(reservation.DateDebut, reservation.DateFin))
            {
                ModelState.AddModelError("", $"La chambre {chambre.Numero} n'est pas disponible du {reservation.DateDebut:dd/MM/yyyy} au {reservation.DateFin:dd/MM/yyyy}.");
                ViewBag.ChambreId = new SelectList(_context.Chambres, "Id", "Numero", reservation.ChambreId);
                ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", reservation.ClientId);
                return View(reservation);
            }

            try
            {
                // Enregistrer la réservation
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                // Charger la chambre pour le calcul du prix
                await _context.Entry(reservation).Reference(r => r.Chambre).LoadAsync();

                // Créer automatiquement la facture associée
                var facture = new Facture
                {
                    ReservationId = reservation.Id,
                    Reservation = reservation
                };
                facture.CalculerTotal();
                _context.Factures.Add(facture);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erreur lors de la création : {ex.Message}");
                ViewBag.ChambreId = new SelectList(_context.Chambres, "Id", "Numero", reservation.ChambreId);
                ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", reservation.ClientId);
                return View(reservation);
            }

            TempData["Success"] = "Réservation créée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Chambre)
                .Include(r => r.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewBag.ChambreId = new SelectList(_context.Chambres, "Id", "Numero", reservation.ChambreId);
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", reservation.ClientId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateDebut,DateFin,ClientId,ChambreId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Success"] = "Réservation modifiée avec succès !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ChambreId = new SelectList(_context.Chambres, "Id", "Numero", reservation.ChambreId);
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", reservation.ClientId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Chambre)
                .Include(r => r.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            TempData["Success"] = "Réservation supprimée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
