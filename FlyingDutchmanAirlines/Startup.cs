using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FlyingDutchmanAirlines;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        // let request routes to endpoint
        app.UseRouting();
        
        // endpoint maps to controller
        app.UseEndpoints(endpoint => endpoint.MapControllers());
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // add all controllers
        services.AddControllers();
        
        // dependency injection
        services.AddTransient(typeof(FlightService), typeof(FlightService));
        services.AddTransient(typeof(BookingService), typeof(BookingService));
        services.AddTransient(typeof(FlightRepository), typeof(FlightRepository));
        services.AddTransient(typeof(AirportRepository), typeof(AirportRepository));
        services.AddTransient(typeof(BookingRepository), typeof(BookingRepository));
        services.AddTransient(typeof(CustomerRepository), typeof(CustomerRepository));
        services.AddTransient(typeof(FlyingDutchmanAirlinesContext), typeof(FlyingDutchmanAirlinesContext));
    }
}