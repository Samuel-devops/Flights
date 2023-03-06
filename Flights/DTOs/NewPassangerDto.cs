namespace Flights.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public record NewPassangerDto(
        [Required][EmailAddress][StringLength(100, MinimumLength = 3)] string Email,
        [Required][MinLength(2)][MaxLength(35)] string FirstName,
        [Required][MinLength(2)][MaxLength(35)] string LastName,
        [Required] bool Gender);
}
