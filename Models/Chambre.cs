using System.ComponentModel.DataAnnotations;

namespace SystemeHotel.Models
{
    public class Chambre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public required string Numero { get; set; }  // required force l'initialisation

        [Required]
        [StringLength(50)]
        public required string Type { get; set; }   // required force l'initialisation

        [Required]
        [Range(0.01, 10000.00)]
        public double Prix { get; set; }

        public StatutChambre Statut { get; set; } = StatutChambre.Disponible;

        // Relations
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        // Méthodes
        public bool EstDisponible() => Statut == StatutChambre.Disponible;

        public bool EstDisponible(DateTime dateDebut, DateTime dateFin)
        {
            return EstDisponible(dateDebut, dateFin, null);
        }

        public bool EstDisponible(DateTime dateDebut, DateTime dateFin, int? excludeReservationId = null)
        {
            if (Statut != StatutChambre.Disponible) return false;

            return !Reservations.Any(r => r.Id != excludeReservationId
                                       && ((dateDebut >= r.DateDebut && dateDebut < r.DateFin) ||
                                           (dateFin > r.DateDebut && dateFin <= r.DateFin) ||
                                           (dateDebut <= r.DateDebut && dateFin >= r.DateFin)));
        }

        public void ChangerStatut(StatutChambre nouveauStatut)
        {
            Statut = nouveauStatut;
        }
    }
}
