using DBFirst.Exceptions;
using DBFirst.Models.DTOs;
using DBFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace DBFirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly IDbService _dbService;

    public TripsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTrips(int page = 1, int pageSize = 10)
    {
        try
        {
            var result = await _dbService.GetTripsAsync(page, pageSize);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("{tripId}/clients")]
    public async Task<IActionResult> GetTripDetails(int tripId, ClientToTripDTO clientToTrip)
    {
        try
        {
            await _dbService.ClientToTripsAsync(tripId, clientToTrip);
            return Ok("Client successfully added");
        }
        catch (ConflictException e)
        {
            Console.WriteLine(e);
            return Conflict(e.Message);
        }
        catch (NotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}