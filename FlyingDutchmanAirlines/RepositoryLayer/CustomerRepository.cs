using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class CustomerRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public CustomerRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CreateCustomer(string name)
    {
        if (IsInvalidCustomerName(name))
        {
            return false;
        }

        var customer = new Customer(name);

        try
        {
            await using (_context)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
        }
        catch
        {
            return false;
        }
        

        return true;
    }

    private bool IsInvalidCustomerName(string name)
    {
        char[] forbiddenCharacters = {'!', '@', '#', '$', '%', '&', '*'};
        return string.IsNullOrEmpty(name) || name.Any(c => forbiddenCharacters.Contains(c));
    }
}