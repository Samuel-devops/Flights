import { Component, OnInit } from '@angular/core';
import { PassangerService } from './../api/services/passanger.service';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-register-passanger',
  templateUrl: './register-passanger.component.html',
  styleUrls: ['./register-passanger.component.css']
})
export class RegisterPassangerComponent implements OnInit {

  constructor(private passangerService: PassangerService,
    private fb: FormBuilder) { }

  form = this.fb.group({
    email: [''],
    firstName: [''],
    lastName: [''],
    isFemale: [true]
  })

  ngOnInit(): void {
  }

  register() {
    console.log("Form Value: ", this.form.value);
    this.passangerService.registerPassanger({ body: this.form.value })
      .subscribe(_ => console.log("send form to server!"));
  }

}
