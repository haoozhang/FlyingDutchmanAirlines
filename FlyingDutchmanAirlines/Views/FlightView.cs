namespace FlyingDutchmanAirlines.Views;

public class FlightView
{
    public string FlightNumber { get; private set; }
    
    public AirportInfo Origin { get; private set; }
    
    public AirportInfo Destination { get; private set; }

    public FlightView(string flightNumber, (string city, string code) origin, (string city, string code) destination)
    {
        FlightNumber = string.IsNullOrEmpty(flightNumber) ? "No flight number found" : flightNumber;
        Origin = new AirportInfo(origin);
        Destination = new AirportInfo(destination);
    }
}

public struct AirportInfo
{
    public string City { get; set; }

    public string Code { get; set; }

    public AirportInfo((string city, string code) airport)
    {
        City = string.IsNullOrEmpty(airport.city) ? "No city found" :airport.city;
        Code = string.IsNullOrEmpty(airport.code) ? "No code found" :airport.code;
    }
}