namespace Flights.Controllers;

using Flights.DTOs;
using Microsoft.AspNetCore.Mvc;
using Flights.ReadModels;

[Route("[controller]")]
[ApiController]
public class PassangerController : ControllerBase
{
    private static readonly IList<NewPassangerDto> Passangers = new List<NewPassangerDto>();

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult Register(NewPassangerDto dto)
    {
        if (Passangers.Contains(dto))
        { return this.BadRequest(); }
        Passangers.Add(dto);
        System.Diagnostics.Debug.WriteLine(Passangers.Count);
        return this.CreatedAtAction(nameof(Find), new { email = dto.Email });
    }

    [HttpGet("{email}")]
    public ActionResult<PassangerRm> Find(string email)
    {
        var passanger = Passangers.FirstOrDefault(p => p.Email == email);
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
