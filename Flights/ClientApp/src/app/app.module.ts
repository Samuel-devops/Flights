import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SearchFlightsComponent } from './search-flights/search-flights.component';
import { BookFlightComponent } from './book-flight/book-flight.component';
import { RegisterPassangerComponent } from './register-passanger/register-passanger.component';
import { MyBookingsComponent } from './my-bookings/my-bookings.component';
import { AuthGuard } from './auth/auth.guard';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    SearchFlightsComponent,
    BookFlightComponent,
    RegisterPassangerComponent,
    MyBookingsComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: SearchFlightsComponent, pathMatch: 'full' },
      { path: 'search-flights', component: SearchFlightsComponent },
      {
        path: 'book-flight/:flightId', component: BookFlightComponent,
        canActivate: [AuthGuard]
      },
      { path: 'register-passanger', component: RegisterPassangerComponent },
      {
        path: 'my-booking', component: MyBookingsComponent,
        canActivate: [AuthGuard] },
    ]),

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
