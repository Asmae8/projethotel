using System.ComponentModel.DataAnnotations;

namespace SystemeHotel.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Telephone { get; set; }

        public Client() { }
    }
}
