using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Extensions;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines.ServiceLayer;

public interface IBookingService
{
    public Task<(bool, Exception?)> CreateBooking(string customerName, int flightNumber);
}

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    private readonly ICustomerRepository _customerRepository;

    private readonly IFlightRepository _flightRepository;
    
    public BookingService() { }

    public BookingService(IBookingRepository bookingRepository, ICustomerRepository customerRepository, IFlightRepository flightRepository)
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _flightRepository = flightRepository;
    }

    public async Task<(bool, Exception?)> CreateBooking(string customerName, int flightNumber)
    {
        if (string.IsNullOrEmpty(customerName) || flightNumber.IsNegative())
        {
            Console.WriteLine(
                $"Argument exception in CreateBooking, customer name = {customerName}, flight number = {flightNumber}");
            return (false, new ArgumentException("Invalid customer name or flight number provided."));
        }
        
        Customer customer;
        try
        {
            customer = await _customerRepository.GetCustomerByName(customerName);
        }
        catch (CustomerNotFoundException e)
        {
            // if customer not found, create new one, then rerun recursively
            await _customerRepository.CreateCustomer(customerName);
            return await CreateBooking(customerName, flightNumber);
        }

        try
        {
            _ = await _flightRepository.GetFlightByFlightNumber(flightNumber) ??
                throw new FlightNotFoundException();
            
            await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
            return (true, null);
        }
        catch (Exception e)
        {
            return (false, e);
        }
    }
}