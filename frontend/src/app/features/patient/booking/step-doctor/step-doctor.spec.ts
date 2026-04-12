import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepDoctor } from './step-doctor';

describe('StepDoctor', () => {
  let component: StepDoctor;
  let fixture: ComponentFixture<StepDoctor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StepDoctor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StepDoctor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
