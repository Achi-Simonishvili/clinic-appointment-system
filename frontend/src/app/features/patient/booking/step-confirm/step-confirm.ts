import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AppointmentService } from '../../../../core/services/appointment';
import { AuthService } from '../../../../core/services/auth';
import { DoctorDto } from '../../../../core/models/doctor.models';

@Component({
  selector: 'app-step-confirm',
  imports: [CommonModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './step-confirm.html',
  styleUrl: './step-confirm.scss',
})
export class StepConfirm {
  @Input() doctor!: DoctorDto;
  @Input() date = '';
  @Input() slot = '';
  @Output() confirmed = new EventEmitter<void>();
  @Output() back = new EventEmitter<void>();

  submitting = false;
  errorMessage = '';

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
  ) {}

  confirm(): void {
    this.submitting = true;
    this.errorMessage = '';
    const patientId = this.authService.getPatientId();

    this.appointmentService
      .book(patientId, {
        doctorId: this.doctor.id,
        appointmentDate: this.date,
        startTime: this.slot,
      })
      .subscribe({
        next: () => {
          this.submitting = false;
          this.confirmed.emit();
        },
        error: (err) => {
          this.errorMessage = err.error?.message ?? 'Failed to book appointment.';
          this.submitting = false;
        },
      });
  }
}
