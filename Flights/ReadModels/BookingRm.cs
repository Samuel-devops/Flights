namespace Flights.ReadModels;

public record BookingRm(
    Guid FlightId,
    string Airline,
    string Price,
    TimePlaceRm Arrival,
    TimePlaceRm Departureee,
    int NumberOfBookedSeats,
    string PassengerEmail);
