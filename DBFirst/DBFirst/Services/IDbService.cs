using DBFirst.Models;
using DBFirst.Models.DTOs;

namespace DBFirst.Services;

public interface IDbService
{
    Task<GetTripsDTO> GetTripsAsync(int page, int pageSize);
    Task DeleteClientAsync(int clientId);
    
    Task ClientToTripsAsync(int tripId, ClientToTripDTO clientToTrip);
    
}