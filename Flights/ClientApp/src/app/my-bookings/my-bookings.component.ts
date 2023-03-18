import { Component, OnInit } from '@angular/core';
import { BookingRm, BookDto } from '../api/models';
import { BookingService } from '../api/services/booking.service';
import { AuthService } from './../auth/auth.service';

@Component({
  selector: 'app-my-bookings',
  templateUrl: './my-bookings.component.html',
  styleUrls: ['./my-bookings.component.css']
})
export class MyBookingsComponent implements OnInit {
  bookings!: BookingRm[];

  constructor(private bs: BookingService,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.bs.listBooking({ email: this.authService.currentUser?.email ?? '' })
      .subscribe(r => this.bookings = r);
  }

  private handleError(err: any) {
    console.log("Response Error, Status: ", err.status);
    console.log("Response Error, Status Text: ", err.statusText);
    console.log(err);
  }

  cancel(booking: BookingRm) {
    const dto: BookDto = {
      flightId: booking.flightId,
      numberOfSeats: booking.numberOfBookedSeats,
      passengerEmail: booking.passengerEmail
    };

    this.bs.cancleBooking({ body: dto })
      .subscribe(_ => this.bookings = this.bookings.filter(b => b != booking)
        , this.handleError)
  }
}
