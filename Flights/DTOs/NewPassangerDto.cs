namespace Flights.DTOs
{
    public record NewPassangerDto(
        string Email,
        string FirstName,
        string LastName,
        bool Gender);
}