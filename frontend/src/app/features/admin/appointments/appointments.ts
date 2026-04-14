import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { AppointmentService } from '../../../core/services/appointment';
import { AppointmentDto } from '../../../core/models/appointment.models';

@Component({
  selector: 'app-appointments',
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatSelectModule,
    ReactiveFormsModule,
  ],
  templateUrl: './appointments.html',
  styleUrl: './appointments.scss',
})
export class Appointments implements OnInit {
  appointments: AppointmentDto[] = [];
  filtered: AppointmentDto[] = [];
  loading = false;
  displayedColumns = ['patient', 'doctor', 'date', 'time', 'status', 'actions'];

  statusFilter = new FormControl('');
  statuses = ['', 'Pending', 'Confirmed', 'Completed', 'Cancelled'];

  constructor(private appointmentService: AppointmentService) {}

  ngOnInit(): void {
    this.load();
    this.statusFilter.valueChanges.subscribe(() => this.applyFilter());
  }

  load(): void {
    this.loading = true;
    this.appointmentService.getAll().subscribe({
      next: (res) => {
        this.appointments = res.items ?? res;
        this.filtered = this.appointments;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  applyFilter(): void {
    const status = this.statusFilter.value;
    this.filtered = status
      ? this.appointments.filter((a) => a.status === status)
      : this.appointments;
  }

  updateStatus(id: string, status: string): void {
    this.appointmentService.updateStatus(id, status).subscribe({
      next: () => this.load(),
    });
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
