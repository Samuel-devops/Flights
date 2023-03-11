namespace Flights.Controllers;

using Flights.DTOs;
using Microsoft.AspNetCore.Mvc;
using Flights.ReadModels;
using Flights.Domain.Entities;

[Route("[controller]")]
[ApiController]
public class PassangerController : ControllerBase
{
    private static readonly IList<Passenger> Passengers = new List<Passenger>();

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult Register(NewPassengerDto dto)
    {
        var passenger = new Passenger(
            dto.Email,
            dto.FirstName,
            dto.LastName,
            dto.Gender);

        if (Passengers.Contains(passenger))
        { return this.BadRequest(); }

        Passengers.Add(passenger);
        System.Diagnostics.Debug.WriteLine(Passengers.Count);
        return this.CreatedAtAction(nameof(Find), new { email = dto.Email });
    }

    [HttpGet("{email}")]
    public ActionResult<PassangerRm> Find(string email)
    {
        var passanger = Passengers.FirstOrDefault(p => p.Email == email);
        if (passanger == null)
        {
            return this.NotFound();
        }

        var rm = new PassangerRm(
            passanger.Email,
            passanger.FirstName,
            passanger.LastName,
            passanger.Gender
            );

        return this.Ok(rm);
    }
}
