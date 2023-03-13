namespace Flights.Controllers;

using Flights.DTOs;
using Microsoft.AspNetCore.Mvc;
using Flights.ReadModels;
using Flights.Domain.Entities;
using Flights.Data;

[Route("[controller]")]
[ApiController]
public class PassangerController : ControllerBase
{
    private readonly Entities entities;

    public PassangerController(Entities entities) => this.entities = entities;

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

        if (this.entities.Passengers.Contains(passenger))
        { return this.BadRequest(); }

        this.entities.Passengers.Add(passenger);
        this.entities.SaveChanges();

        return this.CreatedAtAction(nameof(Find), new { email = dto.Email });
    }

    [HttpGet("{email}")]
    public ActionResult<PassangerRm> Find(string email)
    {
        var passanger = this.entities.Passengers.FirstOrDefault(p => p.Email == email);
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
