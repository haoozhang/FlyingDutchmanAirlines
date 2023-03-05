using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Extensions;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;
    
    public BookingRepository() { }

    public BookingRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public virtual async Task<bool> CreateBooking(int customerId, int flightNumber)
    {
        if (customerId.IsNegative() || flightNumber.IsNegative())
        {
            Console.WriteLine($"Argument Exception in CreateBooking, customerId = {customerId}, flightNumber = {flightNumber}.");
            throw new ArgumentException("Invalid customer id or flight number provided.");
        }

        var booking = new Booking()
        {
            CustomerId = customerId,
            FlightNumber = flightNumber,
        };

        try
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Database Exception in CreateBooking.");
            throw new CouldNotAddBookingToDatabaseException();
        }

        return true;
    }
}