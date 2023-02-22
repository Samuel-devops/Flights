import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterPassangerComponent } from './register-passanger.component';

describe('RegisterPassangerComponent', () => {
  let component: RegisterPassangerComponent;
  let fixture: ComponentFixture<RegisterPassangerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterPassangerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegisterPassangerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
