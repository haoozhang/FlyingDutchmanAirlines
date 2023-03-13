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
    }
}