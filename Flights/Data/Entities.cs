namespace Flights.Data;

using Flights.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class Entities : DbContext
{
    public DbSet<Passenger> Passengers => this.Set<Passenger>();
    public DbSet<Flight> Flights => this.Set<Flight>();

    public Entities(DbContextOptions<Entities> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Passenger>().HasKey(p => p.Email);

        modelBuilder.Entity<Flight>().Property(p => p.RemainingNumberOfSeats)
                                     .IsConcurrencyToken();

        modelBuilder.Entity<Flight>().OwnsOne(f => f.Departure);
        modelBuilder.Entity<Flight>().OwnsOne(f => f.Arrival);

        modelBuilder.Entity<Flight>().OwnsMany(f => f.Bookings);
    }
}
