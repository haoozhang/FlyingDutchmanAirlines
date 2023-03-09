using System.Net;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Extensions;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

[Route("{controller}")]
public class BookingController : Controller
{
    private readonly BookingService _bookingService;

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    [HttpPost("{flightNumber}")]
    public async Task<IActionResult> CreateBooking([FromBody] BookingData body, int flightNumber)
    {
        if (!ModelState.IsValid || flightNumber.IsNegative())
        {
            return StatusCode((int) HttpStatusCode.InternalServerError, ModelState.Root.Errors.First().ErrorMessage);
        }
        
        var customerName = $"{body.FirstName} {body.LastName}";
        (bool result, Exception? exception) = await _bookingService.CreateBooking(customerName, flightNumber);

        if (result && exception == null)
        {
            return StatusCode((int) HttpStatusCode.Created);
        }

        return StatusCode((int) HttpStatusCode.InternalServerError, exception?.Message);
    }
}