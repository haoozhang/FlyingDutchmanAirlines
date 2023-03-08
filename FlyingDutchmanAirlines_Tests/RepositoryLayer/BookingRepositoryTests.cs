using FlyingDutchmanAirlines_Tests.Stubs;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

public class BookingRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;

    private BookingRepository _repository;
    
    [SetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchmanAirlines").Options;
        
        // use stub class to customize the behavior of in-memory database
        _context = new FlyingDutchmanAirlinesContextStub(dbContextOptions);

        _repository = new BookingRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestCase(0, -1)]
    [TestCase(-1, 0)]
    [TestCase(-1, -1)]
    public async Task CreateBooking_Failure_InvalidInputs(int customerId, int flightNumber)
    {
        try
        {
            await _repository.CreateBooking(customerId, flightNumber);
            Assert.Fail("Should throw exception.");
        }
        catch (ArgumentException e)
        {
            Assert.Pass("Argument Exception expected.");
        }
    }

    [Test]
    public async Task CreateBooking_Failure_DatabaseError()
    {
        try
        {
            await _repository.CreateBooking(12345678, 1);
            Assert.Fail("Should throw exception.");
        }
        catch (CouldNotAddBookingToDatabaseException e)
        {
            Assert.Pass("Database Exception expected.");
        }
    }
    
    [Test]
    public async Task CreateBooking_Success()
    {
        var result = await _repository.CreateBooking(1, 1);
        Assert.That(result, Is.True);
    }

    [TearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}