using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;
    
    public FlightRepository() { }

    public FlightRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public virtual async Task<Flight> GetFlightByFlightNumber(int flightNumber)
    {
        if (flightNumber.IsNegative())
        {
            Console.WriteLine($"Argument Exception in GetFlightByFlightNumber, flight number = {flightNumber}.");
            throw new ArgumentException("Invalid flight number provided.");
        }

        return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber) ??
               throw new FlightNotFoundException();
    }
}