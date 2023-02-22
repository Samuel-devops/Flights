namespace Flights.ReadModels
{
    public record PassangerRm(
        string Email,
        string FirstName,
        string LastName,
        bool Gender);
}