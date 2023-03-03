using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

public class AirportRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;

    private AirportRepository _repository;

    [SetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchmanAirlines").Options;
        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);

        _repository = new AirportRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [Test]
    public async Task GetAirportById_Failure_InvalidInput()
    {
        var outputStream = new StringWriter();
        try
        {
            Console.SetOut(outputStream);
            await _repository.GetAirportById(-1);
            Assert.Fail("Argument Exception should be thrown.");
        }
        catch (ArgumentException e)
        {
            Assert.IsTrue(outputStream.ToString().Contains("Argument Exception in GetAirportById, AirportId = -1"));
        }
        finally
        {
            await outputStream.DisposeAsync();
        }
    }

    [Test]
    public async Task GetAirportById_Success()
    {
        var airport = new Airport()
        {
            AirportId = 0,
            City = "test-city",
            Iata = "GH",
        };

        _context.Airports.Add(airport);
        await _context.SaveChangesAsync();

        var result = _repository.GetAirportById(0);
        Assert.IsNotNull(result);
        Assert.That(airport.AirportId, Is.EqualTo(0));
        Assert.That(airport.City, Is.EqualTo("test-city"));
        Assert.That(airport.Iata, Is.EqualTo("GH"));
    }
}