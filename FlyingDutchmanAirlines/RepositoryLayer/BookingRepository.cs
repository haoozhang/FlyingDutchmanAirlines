using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public BookingRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateBooking(int customerId, int flightNumber)
    {
        if (customerId < 0 || flightNumber < 0)
        {
            // log
            throw new ArgumentException("Invalid arguments provided.");
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
            // log
            throw new CouldNotAddBookingToDatabaseException();
        }

        return true;
    }
}