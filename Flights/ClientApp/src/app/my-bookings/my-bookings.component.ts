import { Component, OnInit } from '@angular/core';
import { BookingRm } from '../api/models';
import { BookingService } from '../api/services/booking.service';
import { AuthService } from './../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-bookings',
  templateUrl: './my-bookings.component.html',
  styleUrls: ['./my-bookings.component.css']
})
export class MyBookingsComponent implements OnInit {

  bookings!: BookingRm[];

  constructor(private bs: BookingService,
    private authService: AuthService,
    private router: Router  ) { }

  ngOnInit(): void {

    if (!this.authService.currentUser?.email) {
      this.router.navigate(['/register-passanger'])
    }

    this.bs.listBooking({ email: this.authService.currentUser?.email?? '' })
      .subscribe(r => this.bookings = r);
  }

  private handleError(err: any) {
    console.log("Response Error, Status: ", err.status);
    console.log("Response Error, Status Text: ", err.statusText);
    console.log(err);
  }
}
