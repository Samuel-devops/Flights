namespace Flights.Controllers;

using Flights.Data;
using Flights.ReadModels;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly Entities entities;

    public BookingController(Entities entities) => this.entities = entities;

    [HttpGet("{email}")]
    [ProducesResponseType(500)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(IEnumerable<BookingRm>), 200)]
    public ActionResult<IEnumerable<BookingRm>> List(string email)
    {
        var bookings = entities.Flights.ToArray()
                               .SelectMany(x => x.Bookings
                               .Where(b => b.PassengerEmail == email)
                               .Select(b => new BookingRm(
                                   x.Id,
                                   x.Airline,
                                   x.Price.ToString(),
                                   new TimePlaceRm(x.Arrival.Place, x.Arrival.Time),
                                   new TimePlaceRm(x.Departure.Place, x.Departure.Time),
                                   b.NumberOfSeats,
                                   b.PassengerEmail
                                   )));

        return this.Ok(bookings);
    }
}
