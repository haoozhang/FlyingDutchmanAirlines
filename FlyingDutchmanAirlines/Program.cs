using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FlyingDutchmanAirlines
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            InitializeHost();
        }

        private static void InitializeHost()
        {
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
                {
                    // use startup
                    builder.UseStartup<Startup>();
                    builder.UseUrls("http://0.0.0.0:8080");
                })
                .Build()
                .Run();
        }
    }
}