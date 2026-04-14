import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { AppointmentService } from '../../../core/services/appointment';
import { AuthService } from '../../../core/services/auth';
import { AppointmentDto } from '../../../core/models/appointment.models';

@Component({
  selector: 'app-appointments',
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
  ],
  templateUrl: './appointments.html',
  styleUrl: './appointments.scss',
})
export class Appointments implements OnInit {
  appointments: AppointmentDto[] = [];
  loading = false;
  displayedColumns = ['patient', 'date', 'time', 'status', 'actions'];

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    const doctorId = this.authService.getDoctorId();
    this.appointmentService.getByDoctorId(doctorId).subscribe({
      next: (data) => {
        this.appointments = data;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  updateStatus(id: string, status: string): void {
    this.appointmentService.updateStatus(id, status).subscribe({
      next: () => this.load(),
    });
  }

  getStatusColor(status: string): string {
    const map: Record<string, string> = {
      Pending: 'accent',
      Confirmed: 'primary',
      Completed: '',
      Cancelled: 'warn',
    };
    return map[status] ?? '';
  }

  canConfirm(status: string): boolean {
    return status === 'Pending';
  }
  canComplete(status: string): boolean {
    return status === 'Confirmed';
  }
  canCancel(status: string): boolean {
    return status === 'Pending' || status === 'Confirmed';
  }
}
