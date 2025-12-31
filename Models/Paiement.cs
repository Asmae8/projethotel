using System.ComponentModel.DataAnnotations;

namespace SystemeHotel.Models
{
    public class Paiement
    {
        public int Id { get; set; }

        [Required]
        public double Montant { get; set; }

        [Required]
        public ModePaiement Mode { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DatePaiement { get; set; } = DateTime.Now;

        // Clé étrangère
        public int FactureId { get; set; }

        // Relation
        public virtual Facture? Facture { get; set; }
    }

    public class PaiementEnLigne : Paiement
    {
        [StringLength(100)]
        public string? TransactionId { get; set; }

        public bool VerifierSecurite()
        {
            return !string.IsNullOrEmpty(TransactionId);
        }
    }
}
