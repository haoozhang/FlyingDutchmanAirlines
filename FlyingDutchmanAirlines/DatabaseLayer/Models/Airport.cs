namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public partial class Airport
{
    public int AirportId { get; set; }

    public string City { get; set; } = null!;

    public string Iata { get; set; } = null!;

    public virtual ICollection<Flight> FlightDestinationNavigations { get; } = new List<Flight>();

    public virtual ICollection<Flight> FlightOriginNavigations { get; } = new List<Flight>();
}
