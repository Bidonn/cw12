namespace DBFirst.Models.DTOs;

public class GetTripsDTO
{   
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDto> Trips { get; set; }
}

public class TripDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<CountryDto> Countries { get; set; }
    public List<ClientDto> Clients { get; set; }
}
public class CountryDto
{
    public string Name { get; set; }
}
public class ClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}