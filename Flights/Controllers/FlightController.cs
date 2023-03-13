namespace Flights.Controllers;

using Flights.ReadModels;
using Microsoft.AspNetCore.Mvc;
using System;
using Flights.DTOs;
using Flights.Domain.Entities;
using Flights.Domain.Errors;

[Route("[controller]")]
[ApiController]
public class FlightController : ControllerBase
{
    private readonly ILogger<FlightController> logger;
    private static readonly Random Random = new();

    private static readonly Flight[] Flights = new Flight[]
    {
        new (   Guid.NewGuid(),
            "American Airlines",
            Random.Next(90, 5000).ToString(),
            new TimePlace("Los Angeles",DateTime.Now.AddHours(Random.Next(1, 3))),
            new TimePlace("Istanbul",DateTime.Now.AddHours(Random.Next(4, 10))),
                2),
    new (   Guid.NewGuid(),
            "Deutsche BA",
            Random.Next(90, 5000).ToString(),
            new TimePlace("Munchen",DateTime.Now.AddHours(Random.Next(1, 10))),
            new TimePlace("Schiphol",DateTime.Now.AddHours(Random.Next(4, 15))),
            Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "British Airways",
            Random.Next(90, 5000).ToString(),
            new TimePlace("London, England",DateTime.Now.AddHours(Random.Next(1, 15))),
            new TimePlace("Vizzola-Ticino",DateTime.Now.AddHours(Random.Next(4, 18))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Basiq Air",
            Random.Next(90, 5000).ToString(),
            new TimePlace("Amsterdam",DateTime.Now.AddHours(Random.Next(1, 21))),
            new TimePlace("Glasgow, Scotland",DateTime.Now.AddHours(Random.Next(4, 21))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "BB Heliag",
            Random.Next(90, 5000).ToString(),
            new TimePlace("Zurich",DateTime.Now.AddHours(Random.Next(1, 23))),
            new TimePlace("Baku",DateTime.Now.AddHours(Random.Next(4, 25))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Adria Airways",
            Random.Next(90, 5000).ToString(),
            new TimePlace("Ljubljana",DateTime.Now.AddHours(Random.Next(1, 15))),
            new TimePlace("Warsaw",DateTime.Now.AddHours(Random.Next(4, 19))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "ABA Air",
            Random.Next(90, 5000).ToString(),
            new TimePlace("Praha Ruzyne",DateTime.Now.AddHours(Random.Next(1, 55))),
            new TimePlace("Paris",DateTime.Now.AddHours(Random.Next(4, 58))),
                Random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "AB Corporate Aviation",
            Random.Next(90, 5000).ToString(),
            new TimePlace("Le Bourget",DateTime.Now.AddHours(Random.Next(1, 58))),
            new TimePlace("Zagreb",DateTime.Now.AddHours(Random.Next(4, 60))),
                Random.Next(1, 853))
    };

    public FlightController(ILogger<FlightController> logger) => this.logger = logger;

    [HttpGet]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(IEnumerable<FlightRm>), 200)]
    public IEnumerable<FlightRm> Search()
    {
        var flightRmList = Flights.Select(flight => new FlightRm(
            flight.Id,
            flight.Airline,
            flight.Price,
            new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
            new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
            flight.RemainingNumberOfSeats
            ));

        return flightRmList;
    }

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

        var readModel = new FlightRm(
            flight.Id,
            flight.Airline,
            flight.Price,
            new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
            new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
            flight.RemainingNumberOfSeats
            );

        return this.Ok(readModel);
    }

    [HttpPost]
    [ProducesResponseType(400)]
    [ProducesResponseType(505)]
    [ProducesResponseType(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Book(BookDTO dto)
    {
        System.Diagnostics.Debug.WriteLine($"Booking a new Flight {dto.FlightId}");

        var flight = Flights.SingleOrDefault(f => Equals(f.Id, dto.FlightId));

        if (flight is null)
        {
            return this.NotFound();
        }

        var error = flight.MakeBooking(dto.PassengerEmail, dto.NumberOfSeats);

        if (error is OverbookError)
        {
            return this.Conflict(new { message = $"There are only {flight.RemainingNumberOfSeats} seats remaining!" });
        }

        return this.CreatedAtAction(nameof(Find), new { id = dto.FlightId });
    }
}
