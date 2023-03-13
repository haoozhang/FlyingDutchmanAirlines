using System.Net;
using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer;

public class FlightControllerTests
{
    private Mock<IFlightService> _flightServiceMock;

    private FlightController _flightController;

    [SetUp]
    public void Setup()
    {
        _flightServiceMock = new Mock<IFlightService>();
        _flightController = new FlightController(_flightServiceMock.Object);
    }

    [Test]
    public async Task GetAllFlights_Success()
    {
        var flights = new List<FlightView>()
        {
            new FlightView("111", ("city-111", "code-111"), ("city-222", "code-222")),
            new FlightView("333", ("city-333", "code-333"), ("city-444", "code-444")),
        };
        _flightServiceMock
            .Setup(f => f.GetAllFlights())
            .Returns(FlightViewAsyncGenerator(flights));

        var response = await _flightController.GetFlights() as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        var content = response.Value as Queue<FlightView>;
        Assert.IsNotNull(content);
        Assert.IsTrue(flights.All(f => content.Contains(f)));
    }

    private async IAsyncEnumerable<FlightView> FlightViewAsyncGenerator(List<FlightView> flightViews)
    {
        foreach (var flightView in flightViews)
        {
            yield return flightView;
        }
    }

    [Test]
    public async Task GetAllFlights_FlightNotFound()
    {
        _flightServiceMock
            .Setup(f => f.GetAllFlights())
            .Throws(new FlightNotFoundException());

        var response = await _flightController.GetFlights() as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo((int) HttpStatusCode.NotFound));
        Assert.That(response.Value, Is.EqualTo("No flights found in database."));
    }
    
    [Test]
    public async Task GetAllFlights_InternalServerError()
    {
        _flightServiceMock
            .Setup(f => f.GetAllFlights())
            .Throws(new Exception());

        var response = await _flightController.GetFlights() as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo((int) HttpStatusCode.InternalServerError));
        Assert.That(response.Value, Is.EqualTo("An error Occurred."));
    }
    
    [Test]
    public async Task GetFlightByFlightNumber_Success()
    {
        var flight = new FlightView("111", ("city-111", "code-111"), ("city-222", "code-222"));
        _flightServiceMock
            .Setup(f => f.GetFlightByFlightNumber(0))
            .ReturnsAsync(flight);

        var response = await _flightController.GetFlightByFlightNumber(0) as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        var content = response.Value as FlightView;
        Assert.That(content, Is.EqualTo(flight));
    }

    [Test]
    public async Task GetFlightByFlightNumber_FlightNotFound()
    {
        _flightServiceMock
            .Setup(f => f.GetFlightByFlightNumber(It.IsAny<int>()))
            .Throws(new FlightNotFoundException());

        var response = await _flightController.GetFlightByFlightNumber(1) as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo((int) HttpStatusCode.NotFound));
        Assert.That(response.Value, Is.EqualTo("No flights found in database."));
    }
    
    [Test]
    public async Task GetFlightByFlightNumber_BadRequest()
    {
        _flightServiceMock
            .Setup(f => f.GetFlightByFlightNumber(It.IsAny<int>()))
            .Throws(new Exception());

        var response = await _flightController.GetFlightByFlightNumber(2) as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        Assert.That(response.Value, Is.EqualTo("Bad request."));
    }
}