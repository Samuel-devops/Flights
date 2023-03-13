namespace Flights.Data;

using Flights.Domain.Entities;

public class Entities
{
    public readonly IList<Passenger> Passengers = new List<Passenger>();

    public readonly List<Flight> Flights = new();
}
