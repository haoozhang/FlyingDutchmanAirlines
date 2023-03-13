using Autofac;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;

namespace FlyingDutchmanAirlines;

public class FlyingDutchmanAirlinesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // dependency injection

        builder.RegisterType<FlyingDutchmanAirlinesContext>().AsSelf().SingleInstance().PropertiesAutowired();
        
        builder.RegisterType<FlightRepository>().As<IFlightRepository>().SingleInstance().PropertiesAutowired();
        builder.RegisterType<AirportRepository>().As<IAirportRepository>().SingleInstance().PropertiesAutowired();
        builder.RegisterType<BookingRepository>().As<IBookingRepository>().SingleInstance().PropertiesAutowired();
        builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().SingleInstance().PropertiesAutowired();
        
        builder.RegisterType<FlightService>().As<IFlightService>().SingleInstance().PropertiesAutowired();
        builder.RegisterType<BookingService>().As<IBookingService>().SingleInstance().PropertiesAutowired();
    }
}