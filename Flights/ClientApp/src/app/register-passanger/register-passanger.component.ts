import { Component, OnInit } from '@angular/core';
import { PassangerService } from './../api/services/passanger.service';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-passanger',
  templateUrl: './register-passanger.component.html',
  styleUrls: ['./register-passanger.component.css']
})
export class RegisterPassangerComponent implements OnInit {
  constructor(private passangerService: PassangerService,
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router) { }

  form = this.fb.group({
    email: [''],
    firstName: [''],
    lastName: [''],
    isFemale: [true]
  })

  ngOnInit(): void {
  }

  checkPassenger(): void {
    const params = { email: this.form.get('email')?.value! }
    this.passangerService
      .findPassanger(params)
      .subscribe(
        this.login, e => {
          if (e.status != 404)
            console.error(e)
        }
      )
  }

  register() {
    console.log("Form Value: ", this.form.value);
    this.passangerService.registerPassanger({ body: this.form.value })
      .subscribe(this.login, console.error)
  }

  private login = () => {
    this.authService.loginUser({ email: this.form.get('email')?.value! })
    this.router.navigate(['/search-flights'])
  }
}
