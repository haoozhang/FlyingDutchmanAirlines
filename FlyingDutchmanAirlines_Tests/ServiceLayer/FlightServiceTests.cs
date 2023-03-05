using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

public class FlightServiceTests
{
    private Mock<FlightRepository> _flightRepositoryMock;

    private Mock<AirportRepository> _airportRepositoryMock;

    private FlightService _flightService;

    [SetUp]
    public void Setup()
    {
        _flightRepositoryMock = new Mock<FlightRepository>();

        _airportRepositoryMock = new Mock<AirportRepository>();

        _flightService = new FlightService(_flightRepositoryMock.Object, _airportRepositoryMock.Object);
    }

    [Test]
    public async Task GetAllFlights_Success()
    {
        var flights = new Queue<Flight>();
        flights.Enqueue(new Flight()
        {
            FlightNumber = 111,
            Origin = 222,
            Destination = 333,
        });

        _flightRepositoryMock
            .Setup(f => f.GetAllFlights())
            .ReturnsAsync(flights);

        _airportRepositoryMock
            .Setup(a => a.GetAirportById(222))
            .ReturnsAsync(new Airport()
            {
                AirportId = 222,
                City = "city-222",
                Iata = "code-222",
            });
        _airportRepositoryMock
            .Setup(a => a.GetAirportById(333))
            .ReturnsAsync(new Airport()
            {
                AirportId = 333,
                City = "city-333",
                Iata = "code-333",
            });

        var result = _flightService.GetAllFlights();
        
        Assert.IsNotNull(result);
        await foreach (var flightView in result)
        {
            Assert.IsNotNull(flightView);
            Assert.That(flightView.FlightNumber, Is.EqualTo("111"));
            Assert.That(flightView.Origin.City, Is.EqualTo("city-222"));
            Assert.That(flightView.Origin.Code, Is.EqualTo("code-222"));
            Assert.That(flightView.Destination.City, Is.EqualTo("city-333"));
            Assert.That(flightView.Destination.Code, Is.EqualTo("code-333"));
        }
    }
}