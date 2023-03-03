using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class AirportRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public AirportRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<Airport> GetAirportById(int airportId)
    {
        if (airportId < 0)
        {
            Console.WriteLine($"Argument Exception in GetAirportById, AirportId = {airportId}");
            throw new ArgumentException("Invalid airport id provided.");
        }

        return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == airportId) ??
            throw new AirportNotFoundException();
    }
}