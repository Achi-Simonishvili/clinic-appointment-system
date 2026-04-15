import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MedicalRecordService } from '../../../core/services/medical-record';
import { AppointmentService } from '../../../core/services/appointment';
import { AuthService } from '../../../core/services/auth';
import { MedicalRecordDto } from '../../../core/models/medical-record.models';
import { AppointmentDto } from '../../../core/models/appointment.models';

@Component({
  selector: 'app-medical-records',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatSelectModule,
  ],
  templateUrl: './medical-records.html',
  styleUrl: './medical-records.scss',
})
export class MedicalRecords implements OnInit {
  records: MedicalRecordDto[] = [];
  appointments: AppointmentDto[] = [];
  form: FormGroup;
  loading = false;
  submitting = false;
  errorMessage = '';
  private doctorId = '';

  constructor(
    private fb: FormBuilder,
    private medicalRecordService: MedicalRecordService,
    private appointmentService: AppointmentService,
    private authService: AuthService,
  ) {
    this.form = this.fb.group({
      appointmentId: ['', Validators.required],
      patientId: ['', Validators.required],
      diagnosis: ['', Validators.required],
      symptoms: [''],
      notes: [''],
    });
  }

  ngOnInit(): void {
    this.doctorId = this.authService.getDoctorId();
    this.loadRecords();
    this.loadAppointments();
  }

  loadRecords(): void {
    this.loading = true;
    this.medicalRecordService.getByDoctorId(this.doctorId).subscribe({
      next: (data) => {
        this.records = data;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  loadAppointments(): void {
    this.appointmentService.getByDoctorId(this.doctorId).subscribe({
      next: (data) => {
        this.appointments = data.filter(
          (a) => a.status === 'Confirmed' || a.status === 'Completed',
        );
      },
    });
  }

  onAppointmentChange(appointmentId: string): void {
    const appt = this.appointments.find((a) => a.id === appointmentId);
    if (appt) this.form.patchValue({ patientId: appt.patientId });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.submitting = true;
    this.errorMessage = '';

    this.medicalRecordService.create(this.doctorId, this.form.value).subscribe({
      next: () => {
        this.form.reset();
        this.loadRecords();
        this.submitting = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message ?? 'Failed to create record.';
        this.submitting = false;
      },
    });
  }
}
