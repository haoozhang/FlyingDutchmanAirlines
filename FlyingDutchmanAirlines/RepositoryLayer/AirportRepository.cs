using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public interface IAirportRepository
{
    public Task<Airport> GetAirportById(int airportId);
}

public class AirportRepository : IAirportRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public AirportRepository() { }
    
    public AirportRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<Airport> GetAirportById(int airportId)
    {
        if (airportId.IsNegative())
        {
            Console.WriteLine($"Argument Exception in GetAirportById, airportId = {airportId}.");
            throw new ArgumentException("Invalid airport id provided.");
        }

        return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == airportId) ??
            throw new AirportNotFoundException();
    }
}