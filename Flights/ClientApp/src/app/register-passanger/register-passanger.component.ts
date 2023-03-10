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
    email: ['', Validators.compose([Validators.required, Validators.email, Validators.maxLength(100), Validators.minLength(3)])],
    firstName: ['', Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(35)])],
    lastName: ['', Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(35)])],
    isFemale: [true, Validators.required]
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
    if (this.form.invalid) {
      return;
    }

    console.log("Form Value: ", this.form.value);
    this.passangerService.registerPassanger({ body: this.form.value })
      .subscribe(this.login, console.error)
  }

  private login = () => {
    this.authService.loginUser({ email: this.form.get('email')?.value! })
    this.router.navigate(['/search-flights'])
  }

  get email() {
    return this.form.controls.email
  }
  get firstName() {
    return this.form.controls.firstName
  }
  get lastName() {
    return this.form.controls.lastName
  }
  get isFemale() {
    return this.form.controls.isFemale
  }
}
