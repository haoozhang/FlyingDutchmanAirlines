using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public interface IFlightRepository
{
    public Task<Flight> GetFlightByFlightNumber(int flightNumber);

    public Task<Queue<Flight>> GetAllFlights();
}

public class FlightRepository : IFlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;
    
    public FlightRepository() { }

    public FlightRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<Flight> GetFlightByFlightNumber(int flightNumber)
    {
        if (flightNumber.IsNegative())
        {
            Console.WriteLine($"Argument Exception in GetFlightByFlightNumber, flight number = {flightNumber}.");
            throw new ArgumentException("Invalid flight number provided.");
        }

        return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber) ??
               throw new FlightNotFoundException();
    }

    public async Task<Queue<Flight>> GetAllFlights()
    {
        var flights = new Queue<Flight>();
        await _context.Flights.ForEachAsync(f => flights.Enqueue(f));
        return flights;
    }
}