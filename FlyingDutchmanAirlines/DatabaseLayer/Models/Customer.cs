using FlyingDutchmanAirlines.Utils;

namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public sealed class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; }

    public ICollection<Booking> Bookings { get; }

    public Customer(string name)
    {
        Name = name;
        Bookings = new List<Booking>();
    }

    // operator overload
    public static bool operator ==(Customer x, Customer y)
    {
        var comparer = new CustomerEqualityComparer();
        return comparer.Equals(x, y);
    }

    public static bool operator !=(Customer x, Customer y) => !(x == y);
}
