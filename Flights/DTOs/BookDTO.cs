namespace Flights.DTOs;

using System.ComponentModel.DataAnnotations;

public record BookDTO(
    [Required] Guid FlightId,
    [Required][EmailAddress][StringLength(100, MinimumLength = 3)] string PassengerEmail,
    [Required][Range(1, 254)] byte NumberOfSeats);
