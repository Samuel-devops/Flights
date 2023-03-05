namespace Flights.DTOs
{
    public record BookDTO(Guid FlightId,
        string PassengerEmail,
        byte NumberOfSeats);
}