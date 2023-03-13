using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public interface ICustomerRepository
{
    public Task<bool> CreateCustomer(string name);

    public Task<Customer> GetCustomerByName(string name);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;
    
    public CustomerRepository() { }

    public CustomerRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CreateCustomer(string name)
    {
        if (IsInvalidCustomerName(name))
        {
            Console.WriteLine($"Argument Exception in CreateCustomer, customer name = {name}.");
            throw new ArgumentException("Invalid customer name provided.");
        }

        var customer = new Customer(name);

        try
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Database Exception in CreateCustomer.");
            throw new CouldNotAddCustomerToDatabaseException();
        }

        return true;
    }

    private bool IsInvalidCustomerName(string name)
    {
        char[] forbiddenCharacters = {'!', '@', '#', '$', '%', '&', '*'};
        return string.IsNullOrEmpty(name) || name.Any(c => forbiddenCharacters.Contains(c));
    }

    public async Task<Customer> GetCustomerByName(string name)
    {
        if (IsInvalidCustomerName(name))
        {
            Console.WriteLine($"Argument Exception in GetCustomerByName, customer name = {name}.");
            throw new ArgumentException("Invalid customer name provided.");
        }

        return await _context.Customers.FirstOrDefaultAsync(c => c.Name == name)
               ?? throw new CustomerNotFoundException();
    }
}