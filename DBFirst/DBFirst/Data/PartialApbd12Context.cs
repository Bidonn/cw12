using DBFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Data;

public partial class Apbd12Context : DbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Client>().HasData(
            new Client { IdClient = 1, FirstName = "Anna", LastName = "Kowalska", Email = "anna@example.com", Telephone = "123-456-789", Pesel = "90010112345" },
            new Client { IdClient = 2, FirstName = "Piotr", LastName = "Nowak", Email = "piotr@example.com", Telephone = "234-567-890", Pesel = "88020254321" },
            new Client { IdClient = 3, FirstName = "Maria", LastName = "Wiśniewska", Email = "maria@example.com", Telephone = "345-678-901", Pesel = "92030367890" }
        );
        
        modelBuilder.Entity<Country>().HasData(
            new Country { IdCountry = 1, Name = "Poland" },
            new Country { IdCountry = 2, Name = "Italy" },
            new Country { IdCountry = 3, Name = "Germany" }
        );
        
        modelBuilder.Entity<Trip>().HasData(
            new Trip
            {
                IdTrip = 1,
                Name = "Trip to Krakow",
                Description = "Explore the old city of Krakow",
                DateFrom = new DateTime(2025, 8, 1),
                DateTo = new DateTime(2025, 8, 10),
                MaxPeople = 20
            },
            new Trip
            {
                IdTrip = 2,
                Name = "Trip to Rome",
                Description = "Enjoy the Italian capital",
                DateFrom = new DateTime(2025, 9, 5),
                DateTo = new DateTime(2025, 9, 12),
                MaxPeople = 15
            }
        );
    }

}