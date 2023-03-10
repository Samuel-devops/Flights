namespace Flights.Controllers;

using System;
using Flights.Domain.Errors;
using Flights.DTOs;
using Flights.ReadModels;
using Flights.Data;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class FlightController : ControllerBase
{
    private readonly ILogger<FlightController> logger;

    private readonly Entities entities;

    public FlightController(ILogger<FlightController> logger, Entities entities)
    {
        this.logger = logger;
        this.entities = entities;
    }

    [HttpGet]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(IEnumerable<FlightRm>), 200)]
    public IEnumerable<FlightRm> Search()
    {
        var flightRmList = this.entities.Flights.Select(flight => new FlightRm(
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
        var flight = this.entities.Flights.SingleOrDefault(f => Equals(f.Id, id));
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

        var flight = this.entities.Flights.SingleOrDefault(f => Equals(f.Id, dto.FlightId));

        if (flight is null)
        {
            return this.NotFound();
        }

        var error = flight.MakeBooking(dto.PassengerEmail, dto.NumberOfSeats);

        if (error is OverbookError)
        {
            return this.Conflict(new { message = $"There are only {flight.RemainingNumberOfSeats} seats remaining!" });
        }
        this.entities.SaveChanges();

        return this.CreatedAtAction(nameof(Find), new { id = dto.FlightId });
    }
}
