using DBFirst.Exceptions;
using DBFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace DBFirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IDbService _dbService;

    public ClientController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            await _dbService.DeleteClientAsync(id);
            return Ok($"Client {id} deleted.");
        }
        catch (NotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            Console.WriteLine(e);
            return Conflict(e.Message);
        }
    }
}