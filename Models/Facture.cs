using System.ComponentModel.DataAnnotations;

namespace SystemeHotel.Models
{
    public class Facture
    {
        public int Id { get; set; }

        [Required]
        public double Total { get; set; }

        [Required]
        [StringLength(50)]
        public string Statut { get; set; } = "En attente";

        [DataType(DataType.Date)]
        public DateTime DateEmission { get; set; } = DateTime.Now;

        // Clé étrangère
        public int ReservationId { get; set; }

        // Relations
        public virtual Reservation? Reservation { get; set; }
        public virtual Paiement? Paiement { get; set; }

        // Méthodes
        public void CalculerTotal()
        {
            Total = Reservation?.CalculerPrix() ?? 0;
        }
    }
}
