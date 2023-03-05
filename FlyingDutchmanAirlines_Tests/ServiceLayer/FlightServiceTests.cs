using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
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

    [Test]
    public async Task GetAllFlights_Failure_AirportException()
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
            .ThrowsAsync(new AirportNotFoundException());
        
        try
        {
            var result = _flightService.GetAllFlights();
            // because 'yield return' is used in 'GetAllFlights' method,
            // the result won't be returned immediately until we access it.
            await foreach (var _ in result)
            {
                ;
            }
            Assert.Fail("Exception should be thrown.");
        }
        catch (Exception e)
        {
            Assert.Pass("Exception expected.");
        }
    }

    [Test]
    public async Task GetFlightByFlightNumber_Success()
    {
        var flight = new Flight()
        {
            FlightNumber = 111,
            Origin = 222,
            Destination = 333,
        };

        _flightRepositoryMock
            .Setup(f => f.GetFlightByFlightNumber(111))
            .ReturnsAsync(flight);
        
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

        var result = await _flightService.GetFlightByFlightNumber(111);
        
        Assert.IsNotNull(result);
        Assert.That(result.FlightNumber, Is.EqualTo("111"));
        Assert.That(result.Origin.City, Is.EqualTo("city-222"));
        Assert.That(result.Origin.Code, Is.EqualTo("code-222"));
        Assert.That(result.Destination.City, Is.EqualTo("city-333"));
        Assert.That(result.Destination.Code, Is.EqualTo("code-333"));
    }
    
    [Test]
    public async Task GetFlightByFlightNumber_Failure_FlightNotFound()
    {
        _flightRepositoryMock
            .Setup(f => f.GetFlightByFlightNumber(111))
            .ThrowsAsync(new FlightNotFoundException());

        try
        {
            var result = await _flightService.GetFlightByFlightNumber(111);
            Assert.Fail("Exception should be thrown.");
        }
        catch (FlightNotFoundException e)
        {
            Assert.Pass("Flight Not Found Exception expected.");
        }
    }
    
    [Test]
    public async Task GetFlightByFlightNumber_Failure_ArgumentException()
    {
        var flight = new Flight()
        {
            FlightNumber = 111,
            Origin = 222,
            Destination = 333,
        };

        _flightRepositoryMock
            .Setup(f => f.GetFlightByFlightNumber(111))
            .ReturnsAsync(flight);

        _airportRepositoryMock
            .Setup(a => a.GetAirportById(222))
            .ThrowsAsync(new StackOverflowException());

        try
        {
            var result = await _flightService.GetFlightByFlightNumber(111);
            Assert.Fail("Exception should be thrown.");
        }
        catch (Exception e)
        {
            Assert.Pass("Exception expected.");
        }
    }
}