namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public partial class Flight
{
    public int FlightNumber { get; set; }

    public int Origin { get; set; }

    public int Destination { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual Airport DestinationNavigation { get; set; } = null!;

    public virtual Airport OriginNavigation { get; set; } = null!;
}
