import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { DoctorAvailabilityService } from '../../../core/services/doctor-availability';
import { AuthService } from '../../../core/services/auth';
import { DoctorAvailabilityDto } from '../../../core/models/availability.models';

@Component({
  selector: 'app-availability',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTableModule,
  ],
  templateUrl: './availability.html',
  styleUrl: './availability.scss',
})
export class Availability implements OnInit {
  availabilities: DoctorAvailabilityDto[] = [];
  form: FormGroup;
  loading = false;
  submitting = false;
  errorMessage = '';
  displayedColumns = ['day', 'startTime', 'endTime', 'slotDuration', 'actions'];

  days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

  slotDurations = [
    { label: '15 minutes', value: 15 },
    { label: '30 minutes', value: 30 },
    { label: '45 minutes', value: 45 },
    { label: '60 minutes', value: 60 },
  ];

  private doctorId = '';

  constructor(
    private fb: FormBuilder,
    private availabilityService: DoctorAvailabilityService,
    private authService: AuthService,
  ) {
    this.form = this.fb.group({
      dayOfWeek: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
      slotDurationMinutes: [30, Validators.required],
    });
  }

  ngOnInit(): void {
    this.doctorId = this.authService.getDoctorId();
    this.loadAvailability();
  }

  loadAvailability(): void {
    this.loading = true;
    this.availabilityService.getByDoctorId(this.doctorId).subscribe({
      next: (data) => {
        this.availabilities = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.submitting = true;
    this.errorMessage = '';

    this.availabilityService.setAvailability(this.doctorId, this.form.value).subscribe({
      next: () => {
        this.form.reset({ slotDurationMinutes: 30 });
        this.loadAvailability();
        this.submitting = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message ?? 'Failed to save availability.';
        this.submitting = false;
      },
    });
  }

  delete(id: string): void {
    this.availabilityService.delete(this.doctorId, id).subscribe({
      next: () => this.loadAvailability(),
    });
  }
}
