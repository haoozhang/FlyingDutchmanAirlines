using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

public class BookingServiceTests
{
    private Mock<BookingRepository> _bookingRepositoryMock;

    private Mock<CustomerRepository> _customerRepositoryMock;

    private BookingService _bookingService;

    [SetUp]
    public void Setup()
    {
        _bookingRepositoryMock = new Mock<BookingRepository>();
        _customerRepositoryMock = new Mock<CustomerRepository>();
        _bookingService = new BookingService(_bookingRepositoryMock.Object, _customerRepositoryMock.Object);
    }

    [Test]
    public async Task CreateBooking_Success()
    {
        _bookingRepositoryMock
            .Setup(b => b.CreateBooking(0, 0))
            .ReturnsAsync(true);
        
        _customerRepositoryMock
            .Setup(c => c.GetCustomerByName("0"))
            .ReturnsAsync(new Customer("0") {CustomerId = 0});
        
        var result = await _bookingService.CreateBooking("0", 0);
        Assert.IsTrue(result.Item1);
        Assert.IsNull(result.Item2);
    }
    
    [TestCase(null, 0)]
    [TestCase("", 0)]
    [TestCase("test", -1)]
    public async Task CreateBooking_Failure_InvalidInput(string customerName, int flightNumber)
    {
        var result = await _bookingService.CreateBooking(customerName, flightNumber);
        Assert.IsFalse(result.Item1);
        Assert.IsNotNull(result.Item2);
        Assert.IsInstanceOf(typeof(ArgumentException), result.Item2);
    }

    [Test]
    public async Task CreateBooking_Failure_DatabaseException()
    {
        _bookingRepositoryMock
            .Setup(b => b.CreateBooking(1, 1))
            .ThrowsAsync(new CouldNotAddBookingToDatabaseException());
        
        _customerRepositoryMock
            .Setup(c => c.GetCustomerByName("1"))
            .ReturnsAsync(new Customer("1") {CustomerId = 1});
        
        var result = await _bookingService.CreateBooking("1", 1);
        Assert.IsFalse(result.Item1);
        Assert.IsNotNull(result.Item2);
        Assert.IsInstanceOf(typeof(CouldNotAddBookingToDatabaseException), result.Item2);
    }
}