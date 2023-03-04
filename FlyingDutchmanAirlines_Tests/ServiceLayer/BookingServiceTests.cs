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
        _bookingRepositoryMock
            .Setup(b => b.CreateBooking(0, 0))
            .ReturnsAsync(true);
        _bookingRepositoryMock
            .Setup(b => b.CreateBooking(1, 1))
            .ThrowsAsync(new CouldNotAddBookingToDatabaseException());
        
        _customerRepositoryMock = new Mock<CustomerRepository>();
        _customerRepositoryMock
            .Setup(c => c.GetCustomerByName("0"))
            .ReturnsAsync(new Customer("0") {CustomerId = 0});
        _customerRepositoryMock
            .Setup(c => c.GetCustomerByName("1"))
            .ReturnsAsync(new Customer("1") {CustomerId = 1});

        _bookingService = new BookingService(_bookingRepositoryMock.Object, _customerRepositoryMock.Object);
    }

    [Test]
    public async Task CreateBooking_Success()
    {
        var result = await _bookingService.CreateBooking("0", 0);
        Assert.IsTrue(result.Item1);
        Assert.IsNull(result.Item2);
    }

    [Test]
    public async Task CreateBooking_Failure_DatabaseException()
    {
        var result = await _bookingService.CreateBooking("1", 1);
        Assert.IsFalse(result.Item1);
        Assert.IsNotNull(result.Item2);
    }
}