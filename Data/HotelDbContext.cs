using Microsoft.EntityFrameworkCore;
using SystemeHotel.Models;

namespace SystemeHotel;


    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Chambre> Chambres { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Paiement> Paiements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Clients
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Nom = "Dupont", Email = "dupont@example.com", Telephone = "0123456789" },
                new Client { Id = 2, Nom = "Martin", Email = "martin@example.com", Telephone = "0987654321" },
                new Client { Id = 3, Nom = "Durand", Email = "durand@example.com", Telephone = "0567891234" }
            );

            // Seed data for Chambres
            modelBuilder.Entity<Chambre>().HasData(
                new Chambre { Id = 1, Numero = "101", Type = "Simple", Prix = 50.0, Statut = StatutChambre.Disponible },
                new Chambre { Id = 2, Numero = "102", Type = "Double", Prix = 80.0, Statut = StatutChambre.Disponible },
                new Chambre { Id = 3, Numero = "201", Type = "Suite", Prix = 150.0, Statut = StatutChambre.Disponible }
            );

            // Seed data for Reservations
            modelBuilder.Entity<Reservation>().HasData(
                new Reservation { Id = 1, DateDebut = new DateTime(2025, 12, 27), DateFin = new DateTime(2025, 12, 29), ClientId = 1, ChambreId = 1 },
                new Reservation { Id = 2, DateDebut = new DateTime(2025, 12, 30), DateFin = new DateTime(2026, 1, 2), ClientId = 2, ChambreId = 2 },
                new Reservation { Id = 3, DateDebut = new DateTime(2026, 1, 5), DateFin = new DateTime(2026, 1, 7), ClientId = 3, ChambreId = 3 }
            );
        }
    }

