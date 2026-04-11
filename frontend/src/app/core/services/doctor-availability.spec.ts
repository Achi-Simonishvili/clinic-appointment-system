import { TestBed } from '@angular/core/testing';

import { DoctorAvailability } from './doctor-availability';

describe('DoctorAvailability', () => {
  let service: DoctorAvailability;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DoctorAvailability);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
