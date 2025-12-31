namespace SystemeHotel.Models
{
    public enum StatutChambre
    {
        Disponible,
        Occupee,
        EnMaintenance
    }

    public enum StatutReservation
    {
        EnAttente,
        Confirmee,
        Annulee,
        Terminee
    }

    public enum ModePaiement
    {
        Especes,
        CarteBancaire,
        EnLigne
    }
}
