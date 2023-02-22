using Flights.DTOs;
using Microsoft.AspNetCore.Mvc;
using Flights.ReadModels;

namespace Flights.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PassangerController : ControllerBase
    {
        private static IList<NewPassangerDto> Passangers = new List<NewPassangerDto>();

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Register(NewPassangerDto dto)
        {
            if (Passangers.Contains(dto)) { return BadRequest(); }
            Passangers.Add(dto);
            System.Diagnostics.Debug.WriteLine(Passangers.Count);
            return CreatedAtAction(nameof(Find), new { email = dto.Email });
        }

        [HttpGet("{email}")]
        public ActionResult<PassangerRm> Find(string email)
        {
            var passanger = Passangers.FirstOrDefault(p => p.Email == email);
            if (passanger == null)
            {
                return NotFound();
            }

            var rm = new PassangerRm(
                passanger.Email,
                passanger.FirstName,
                passanger.LastName,
                passanger.Gender
                );

            return Ok(rm);
        }
    }
}