using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines.ServiceLayer;

public interface IFlightService
{
    public IAsyncEnumerable<FlightView> GetAllFlights();

    public Task<FlightView> GetFlightByFlightNumber(int flightNumber);
}

public class FlightService : IFlightService
{
    private IFlightRepository _flightRepository;

    private IAirportRepository _airportRepository;

    public FlightService() { }

    public FlightService(IFlightRepository flightRepository, IAirportRepository airportRepository)
    {
        _flightRepository = flightRepository;
        _airportRepository = airportRepository;
    }

    public async IAsyncEnumerable<FlightView> GetAllFlights()
    {
        var flights = await _flightRepository.GetAllFlights();
        foreach (var flight in flights)
        {
            Airport originAirport;
            Airport destinationAirport;
            try
            {
                originAirport = await _airportRepository.GetAirportById(flight.Origin);
                destinationAirport = await _airportRepository.GetAirportById(flight.Destination);
            }
            catch (Exception e)
            {
                throw new Exception("Exception happened in GetAllFlights", e);
            }

            yield return new FlightView(flight.FlightNumber.ToString(), (originAirport.City, originAirport.Iata),
                (destinationAirport.City, destinationAirport.Iata));
        }
    }

    public async Task<FlightView> GetFlightByFlightNumber(int flightNumber)
    {
        try
        {
            var flight = await _flightRepository.GetFlightByFlightNumber(flightNumber);
            var originAirport = await _airportRepository.GetAirportById(flight.Origin);
            var destinationAirport = await _airportRepository.GetAirportById(flight.Destination);

            return new FlightView(flight.FlightNumber.ToString(), (originAirport.City, originAirport.Iata),
                (destinationAirport.City, destinationAirport.Iata));
        }
        catch (FlightNotFoundException e)
        {
            throw new FlightNotFoundException();
        }
        catch (Exception e)
        {
            throw new ArgumentException();
        }
    }
}