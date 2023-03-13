using System.Net;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Extensions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

[Route("{controller}")]
public class FlightController : Controller
{
    private readonly IFlightService _flightService;

    public FlightController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFlights()
    {
        try
        {
            var flights = new Queue<FlightView>();
            await foreach (var flight in _flightService.GetAllFlights())
            {
                flights.Enqueue(flight);
            }
            return StatusCode((int) HttpStatusCode.OK, flights);
        }
        catch (FlightNotFoundException)
        {
            return StatusCode((int) HttpStatusCode.NotFound, "No flights found in database.");
        }
        catch (Exception)
        {
            return StatusCode((int) HttpStatusCode.InternalServerError, "An error Occurred.");
        }
    }

    [HttpGet("{flightNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFlightByFlightNumber(int flightNumber)
    {
        try
        {
            if (flightNumber.IsNegative())
            {
                throw new ArgumentException();
            }

            var flight = await _flightService.GetFlightByFlightNumber(flightNumber);
            return StatusCode((int) HttpStatusCode.OK, flight);
        }
        catch (FlightNotFoundException)
        {
            return StatusCode((int) HttpStatusCode.NotFound, "No flights found in database.");
        }
        catch (Exception)
        {
            return StatusCode((int) HttpStatusCode.BadRequest, "Bad request.");
        }
    }
}