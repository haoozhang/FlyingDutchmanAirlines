using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class BookingService
{
    private BookingRepository _bookingRepository;

    private CustomerRepository _customerRepository;

    public BookingService(BookingRepository bookingRepository, CustomerRepository customerRepository)
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
    }

    public async Task<(bool, Exception?)> CreateBooking(string customerName, int flightNumber)
    {
        Customer customer;
        try
        {
            customer = await _customerRepository.GetCustomerByName(customerName);
        }
        catch (CustomerNotFoundException e)
        {
            // if customer not found, create new one.
            await _customerRepository.CreateCustomer(customerName);
            // rerun by using recursive
            return await CreateBooking(customerName, flightNumber);
        }

        try
        {
            await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
            return (true, null);
        }
        catch (Exception e)
        {
            return (false, e);
        }
    }
}