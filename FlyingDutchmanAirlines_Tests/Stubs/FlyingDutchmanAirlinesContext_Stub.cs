using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.Stubs;

/// <summary>
/// This class inherits from the original class, and to customize some behaviors.
/// </summary>
public class FlyingDutchmanAirlinesContextStub : FlyingDutchmanAirlinesContext
{
    public FlyingDutchmanAirlinesContextStub(DbContextOptions<FlyingDutchmanAirlinesContext> options)
        : base(options)
    {
        
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // query for pending changes in EF Core
        var pendingChanges = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added);
        var bookings = pendingChanges.Select(e => e.Entity)
            .OfType<Booking>();
        
        // if the customer id = 12345678, throw exception
        if (bookings.Any(b=> b.CustomerId == 12345678))
        {
            throw new Exception("Database Error.");
        }

        await base.SaveChangesAsync(cancellationToken);
        return 0;
    }
    
}