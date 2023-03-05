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

    [TestCase(0, -1)]
    [TestCase(-1, 0)]
    public async Task GetFlightByFlightNumber_Failure_InvalidAirportId(int origin, int destination)
    {
        try
        {
            await _repository.GetFlightByFlightNumber(0, origin, destination);
            Assert.Fail("Should throw exception.");
        }
        catch (ArgumentException e)
        {
            Assert.Pass("Argument Exception expected.");
        }
    }

    [Test]
    public async Task GetFlightByFlightNumber_Failure_InvalidFlightNumber()
    {
        try
        {
            await _repository.GetFlightByFlightNumber(-1, 0, 0);
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

        var result = await _repository.GetFlightByFlightNumber(1, 1, 1);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.FlightNumber);
        Assert.AreEqual(1, result.Origin);
        Assert.AreEqual(1, result.Destination);
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
            var result = await _repository.GetFlightByFlightNumber(1, 1, 1);
            Assert.Fail("Should throw exception.");
        }
        catch (FlightNotFoundException e)
        {
            Assert.Pass("Flight Not Found Exception expected.");
        }
        
        
    }
}