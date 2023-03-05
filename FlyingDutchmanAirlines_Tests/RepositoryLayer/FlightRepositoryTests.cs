using FlyingDutchmanAirlines_Tests.Stubs;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

public class FlightRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;

    private FlightRepository _repository;
    
    [SetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchmanAirlines").Options;
        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);

        _repository = new FlightRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [Test]
    public async Task GetFlightByFlightNumber_Failure_InvalidFlightNumber()
    {
        try
        {
            await _repository.GetFlightByFlightNumber(-1);
            Assert.Fail("Should throw exception.");
        }
        catch (ArgumentException e)
        {
            Assert.Pass("Argument Exception expected.");
        }
    }

    [Test]
    public async Task GetFlightByFlightNumber_Success()
    {
        var flight = new Flight()
        {
            FlightNumber = 1,
            Origin = 1,
            Destination = 1,
        };

        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();

        var result = await _repository.GetFlightByFlightNumber(1);
        
        Assert.IsNotNull(result);
        Assert.That(result.FlightNumber, Is.EqualTo(1));
        Assert.That(result.Origin, Is.EqualTo(1));
        Assert.That(result.Destination, Is.EqualTo(1));
    }

    [Test]
    public async Task GetFlightByFlightNumber_Failure_DatabaseException()
    {
        var flight = new Flight()
        {
            FlightNumber = 2,
            Origin = 2,
            Destination = 2,
        };

        _context.Flights.Add(flight);

        try
        {
            var result = await _repository.GetFlightByFlightNumber(1);
            Assert.Fail("Should throw exception.");
        }
        catch (FlightNotFoundException e)
        {
            Assert.Pass("Flight Not Found Exception expected.");
        }
        
        
    }
}