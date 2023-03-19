namespace Flights.DTOs;

using System.ComponentModel;

public record FlightSearchParameters(

    [DefaultValue("12/25/2022 10:30:00 AM")]
    DateTime? FromDate,

    [DefaultValue("12/25/2022 10:30:00 AM")]
    DateTime? ToDate,

    [DefaultValue("Berlin")]
    string? From,

    [DefaultValue("London")]
    string? Destination,

    [DefaultValue(1)]
    int? NumberOfPassengers
    );
