namespace Flights.Controllers;

using System;
using Flights.Domain.Errors;
using Flights.DTOs;
using Flights.ReadModels;
using Flights.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Flights.Domain.Entities;

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
    public IEnumerable<FlightRm> Search([FromQuery] FlightSearchParameters @params)
    {
        this.logger.LogInformation($"Searching flight: {@params.Destination}");

        // All the Flights from Database
        IQueryable<Flight> flights = this.entities.Flights;

        if (!string.IsNullOrWhiteSpace(@params.Destination))
        {
            flights = flights.Where(f => f.Arrival.Place.Contains(@params.Destination));
        }

        if (!string.IsNullOrWhiteSpace(@params.From))
        {
            flights = flights.Where(f => f.Departure.Place.Contains(@params.From));
        }

        if (@params.FromDate is not null)
        {
            flights = flights.Where(f => f.Departure.Time >= @params.FromDate.Value.Date);
        }

        if (@params.ToDate is not null)
        {
            flights = flights.Where(f => f.Departure.Time <= @params.ToDate.Value.Date.AddDays(1).AddTicks(-1));
        }

        if (@params.NumberOfPassengers is not null and not 0)
        {
            flights = flights.Where(f => f.RemainingNumberOfSeats >= @params.NumberOfPassengers);
        }
        else
        {
            flights = flights.Where(f => f.RemainingNumberOfSeats >= 1);
        }

        var flightRmList = flights
            .Select(flight => new FlightRm(
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

        try
        {
            this.entities.SaveChanges();
        }
        catch (DBConcurrencyException)
        {
            return this.Conflict(new { message = $"An Error is ocurred during Booking, please try again!" });
        }

        return this.CreatedAtAction(nameof(Find), new { id = dto.FlightId });
    }
}
