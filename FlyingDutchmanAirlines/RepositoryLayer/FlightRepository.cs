using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public FlightRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<Flight> GetFlightByFlightNumber(int flightNumber, int originAirportId, int destinationAirportId)
    {
        if (flightNumber.IsNegative())
        {
            throw new FlightNotFoundException();
        }

        if (originAirportId.IsNegative() || destinationAirportId.IsNegative())
        {
            throw new ArgumentException("Invalid airport id provided.");
        }

        return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber) ??
               throw new FlightNotFoundException();
    }
}