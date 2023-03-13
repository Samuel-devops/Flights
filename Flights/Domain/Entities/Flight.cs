namespace Flights.Domain.Entities

{
    using Flights.Domain.Errors;

    public record Flight(
        Guid Id,
        string Airline,
        string Price,
        TimePlace Departure,
        TimePlace Arrival,
        int RemainingNumberOfSeats
        )
    {
        public IList<Booking> Bookings = new List<Booking>();
        public int RemainingNumberOfSeats { get; set; } = RemainingNumberOfSeats;
        public object? MakeBooking(string passengerEmail, byte numberOfSeats)
        {
            var flight = this;
            if (flight.RemainingNumberOfSeats < numberOfSeats)
            {
                return new OverbookError();
            }

            var booking = new Booking(
                passengerEmail,
                numberOfSeats);

            flight.Bookings.Add(booking);

            flight.RemainingNumberOfSeats -= numberOfSeats;
            return null;
        }
    }
}
