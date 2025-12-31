using System.ComponentModel.DataAnnotations;

namespace SystemeHotel.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date d'arrivée")]
        public DateTime DateDebut { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date de départ")]
        public DateTime DateFin { get; set; }

        // Clés étrangères
        [Required(ErrorMessage = "Le client est requis.")]
        public int ClientId { get; set; }
        [Required(ErrorMessage = "La chambre est requise.")]
        public int ChambreId { get; set; }

        // Relations (EF Core)
        public virtual Client Client { get; set; } = null!;
        public virtual Chambre Chambre { get; set; } = null!;
        public virtual Facture? Facture { get; set; }

        // Méthodes
        public double CalculerPrix()
        {
            var nbJours = (DateFin - DateDebut).Days;
            return nbJours * Chambre.Prix;
        }
    }
}
