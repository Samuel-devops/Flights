namespace Flights.Controllers;

using Flights.ReadModels;
using Microsoft.AspNetCore.Mvc;
using System;
using Flights.DTOs;

[Route("[controller]")]
[ApiController]
public class FlightController : ControllerBase
{
    private readonly ILogger<FlightController> logger;
    private static readonly Random Random = new();

    private static readonly FlightRm[] Flights = new FlightRm[]
    {
        new (   Guid.NewGuid(),
            "American Airlines",
            Random.Next(90, 5000).ToString(),
            new TimePlaceRm("Los Angeles",DateTime.Now.AddHours(Random.Next(1, 3))),
            new TimePlaceRm("Istanbul",DateTime.Now.AddHours(Random.Next(4, 10))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Deutsche BA",
            Random.Next(90, 5000).ToString(),
            new TimePlaceRm("Munchen",DateTime.Now.AddHours(Random.Next(1, 10))),
            new TimePlaceRm("Schiphol",DateTime.Now.AddHours(Random.Next(4, 15))),
            Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "British Airways",
            Random.Next(90, 5000).ToString(),
            new TimePlaceRm("London, England",DateTime.Now.AddHours(Random.Next(1, 15))),
            new TimePlaceRm("Vizzola-Ticino",DateTime.Now.AddHours(Random.Next(4, 18))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Basiq Air",
            Random.Next(90, 5000).ToString(),
            new TimePlaceRm("Amsterdam",DateTime.Now.AddHours(Random.Next(1, 21))),
            new TimePlaceRm("Glasgow, Scotland",DateTime.Now.AddHours(Random.Next(4, 21))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "BB Heliag",
            Random.Next(90, 5000).ToString(),
            new TimePlaceRm("Zurich",DateTime.Now.AddHours(Random.Next(1, 23))),
            new TimePlaceRm("Baku",DateTime.Now.AddHours(Random.Next(4, 25))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Adria Airways",
            Random.Next(90, 5000).ToString(),
            new TimePlaceRm("Ljubljana",DateTime.Now.AddHours(Random.Next(1, 15))),
            new TimePlaceRm("Warsaw",DateTime.Now.AddHours(Random.Next(4, 19))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "ABA Air",
            Random.Next(90, 5000).ToString(),
            new TimePlaceRm("Praha Ruzyne",DateTime.Now.AddHours(Random.Next(1, 55))),
            new TimePlaceRm("Paris",DateTime.Now.AddHours(Random.Next(4, 58))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "AB Corporate Aviation",
            Random.Next(90, 5000).ToString(),
            new TimePlaceRm("Le Bourget",DateTime.Now.AddHours(Random.Next(1, 58))),
            new TimePlaceRm("Zagreb",DateTime.Now.AddHours(Random.Next(4, 60))),
                Random.Next(1, 853))
    };

    private static readonly IList<BookDTO> Bookings = new List<BookDTO>();

    public FlightController(ILogger<FlightController> logger) => this.logger = logger;

    [HttpGet]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(IEnumerable<FlightRm>), 200)]
    public IEnumerable<FlightRm> Search()
        => Flights;

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(FlightRm), 200)]
    [HttpGet("{id}")]
    public ActionResult<FlightRm> Find(Guid id)
    {
        var flight = Flights.SingleOrDefault(f => Equals(f.Id, id));
        if (flight is null)
        {
            return this.NotFound();
        }

        return this.Ok(flight);
    }

    [HttpPost]
    [ProducesResponseType(400)]
    [ProducesResponseType(505)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(200)]
    public IActionResult Book(BookDTO dTO)
    {
        System.Diagnostics.Debug.WriteLine($"Booking a new Flight {dTO.FlightId}");

        var flightFound = Flights.Any(f => Equals(f.Id, dTO.FlightId));

        if (flightFound is false)
        {
            return this.NotFound();
        }

        Bookings.Add(dTO);
        return this.CreatedAtAction(nameof(Find), new { id = dTO.FlightId });
    }
}
