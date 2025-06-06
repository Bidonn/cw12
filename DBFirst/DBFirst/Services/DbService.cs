using DBFirst.Data;
using DBFirst.Exceptions;
using DBFirst.Models;
using DBFirst.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Services;

public class DbService : IDbService
{
    private Apbd12Context _dbContext;

    public DbService(Apbd12Context dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<GetTripsDTO> GetTripsAsync(int page, int pageSize)
    {
        { 
            var allTrips = await _dbContext.Trips.CountAsync();

            int allPages = (int)Math.Ceiling((allTrips / (double)pageSize));

            var trips = await _dbContext.Trips
                .Include(t => t.IdCountries)
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize) //pominie odpowiednią ilość stron
                .Take(pageSize) //weźmie tylko "pageSize" ilość stron
                .Select(t => new TripDto
                {
                    Name = t.Name,
                    Description = t.Description,
                    DateFrom = t.DateFrom,
                    DateTo = t.DateTo,
                    MaxPeople = t.MaxPeople,
                    Countries = t.IdCountries.Select(c => new CountryDto
                    {
                        Name = c.Name
                    }).ToList(),
                    Clients = t.ClientTrips.Select(ct => new ClientDto
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    }).ToList()
                })
                .ToListAsync();

            return new GetTripsDTO()
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = allPages,
                Trips = trips
            };
        }
    }

    public async Task DeleteClientAsync(int clientId)
    {
        var client = await _dbContext.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == clientId);

        if (client == null)
        {
            throw new NotFoundException("Client not found.");
        }

        if (client.ClientTrips.Any())
        {
            throw new ConflictException("Client is assigned to at least one trip, either delete or update trip.");
        }

        _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ClientToTripsAsync(int tripId, ClientToTripDTO clientToTrip)
    {
        var trip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.IdTrip == tripId);
        if (trip == null)
            throw new NotFoundException("Trip not found.");

        if (trip.DateFrom <= DateTime.Now)
            throw new ConflictException("Trip already ended.");
        
        var existingClient = await _dbContext.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.Pesel == clientToTrip.Pesel);

        /*
         Tutaj wydaje mi się, że jest błąd w zadaniu, i miało być że gdyby istniaj już
         taki klient, to mamy użyć danych tego sitniejącego, a jeśli nie to go dodać.
         Poleciałem jednak zgodnie z poleceniem i nie można dodać do wycieczki klienta, który 
         znajduje się już w bazie danych. Po prostu zrobiłem i nie zastanawiałem się nad tym, teraz dopiero
         widzę że to trochę dziwnie wygląda...
         */
        if (existingClient != null)
        {
            bool alreadyAssigned = await _dbContext.ClientTrips
                .AnyAsync(ct => ct.IdClient == existingClient.IdClient && ct.IdTrip == tripId);

            if (alreadyAssigned)
                throw new ConflictException("Client already registered for this trip.");
            
            throw new ConflictException("Client with this PESEL already exists.");
        }
        
        var client = new Client
        {
            FirstName = clientToTrip.FirstName,
            LastName = clientToTrip.LastName,
            Email = clientToTrip.Email,
            Telephone = clientToTrip.Telephone,
            Pesel = clientToTrip.Pesel
        };
        
        _dbContext.Clients.Add(client);
        
        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = tripId,
            PaymentDate = clientToTrip.PaymentDate,
            RegisteredAt = DateTime.Now,
            IdClientNavigation = client,
            IdTripNavigation = trip
        };

        _dbContext.ClientTrips.Add(clientTrip);
        await _dbContext.SaveChangesAsync();
    }
}
    