using System.Net;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Extensions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

public class FlightController : Controller
{
    private FlightService _flightService;

    public FlightController(FlightService flightService)
    {
        _flightService = flightService;
    }

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
        catch (FlightNotFoundException e)
        {
            return StatusCode((int) HttpStatusCode.NotFound, "No flights found in database.");
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.InternalServerError, "An error Occurred.");
        }
    }

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
        catch (FlightNotFoundException e)
        {
            return StatusCode((int) HttpStatusCode.NotFound, "No flights found in database.");
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest, "Bad request.");
        }
    }
}