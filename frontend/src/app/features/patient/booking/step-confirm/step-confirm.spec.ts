import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepConfirm } from './step-confirm';

describe('StepConfirm', () => {
  let component: StepConfirm;
  let fixture: ComponentFixture<StepConfirm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StepConfirm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StepConfirm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
