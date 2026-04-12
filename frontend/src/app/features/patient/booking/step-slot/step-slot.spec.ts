import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepSlot } from './step-slot';

describe('StepSlot', () => {
  let component: StepSlot;
  let fixture: ComponentFixture<StepSlot>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StepSlot]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StepSlot);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
