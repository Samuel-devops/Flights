namespace Flights.Domain.Entities

{
    using Flights.Domain.Errors;

    public class Flight
    {
        public Guid Id { get; set; }
        public string Airline { get; set; }
        public string Price { get; set; }
        public TimePlace Departure { get; set; }
        public TimePlace Arrival { get; set; }
        public int RemainingNumberOfSeats { get; set; }

        public IList<Booking> Bookings = new List<Booking>();

        public Flight()
        {
        }

        public Flight(
         Guid id,
         string airline,
         string price,
         TimePlace departure,
         TimePlace arrival,
         int remainingNumberOfSeats
         )
        {
            this.Id = id;
            this.Airline = airline;
            this.Price = price;
            this.Departure = departure;
            this.Arrival = arrival;
            this.RemainingNumberOfSeats = remainingNumberOfSeats;
        }

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
