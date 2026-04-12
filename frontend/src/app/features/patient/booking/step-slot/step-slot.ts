import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DoctorAvailabilityService } from '../../../../core/services/doctor-availability';
import { DoctorAvailabilityDto } from '../../../../core/models/availability.models';
import { DoctorDto } from '../../../../core/models/doctor.models';

@Component({
  selector: 'app-step-slot',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './step-slot.html',
  styleUrl: './step-slot.scss',
})
export class StepSlot implements OnInit {
  @Input() doctor!: DoctorDto;
  @Output() slotSelected = new EventEmitter<{ date: string; slot: string }>();
  @Output() back = new EventEmitter<void>();

  availability: DoctorAvailabilityDto[] = [];
  slots: string[] = [];
  loading = false;

  dateControl = new FormControl('', Validators.required);
  slotControl = new FormControl('', Validators.required);

  constructor(private availabilityService: DoctorAvailabilityService) {}

  ngOnInit(): void {
    this.loading = true;
    this.availabilityService.getByDoctorId(this.doctor.id).subscribe({
      next: (data) => {
        this.availability = data;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });

    this.dateControl.valueChanges.subscribe((date) => {
      if (date) this.generateSlots(date);
    });
  }

  generateSlots(date: string): void {
    const dayName = new Date(date).toLocaleDateString('en-US', { weekday: 'long' });
    const avail = this.availability.find((a) => a.dayOfWeek === dayName);
    if (!avail) {
      this.slots = [];
      return;
    }

    const slots: string[] = [];
    let [h, m] = avail.startTime.split(':').map(Number);
    const [endH, endM] = avail.endTime.split(':').map(Number);
    const endMinutes = endH * 60 + endM;

    while (h * 60 + m < endMinutes) {
      slots.push(`${String(h).padStart(2, '0')}:${String(m).padStart(2, '0')}`);
      m += avail.slotDurationMinutes;
      if (m >= 60) {
        h += Math.floor(m / 60);
        m = m % 60;
      }
    }
    this.slots = slots;
  }

  onNext(): void {
    if (!this.dateControl.value || !this.slotControl.value) return;
    this.slotSelected.emit({ date: this.dateControl.value, slot: this.slotControl.value });
  }

  get today(): string {
    return new Date().toISOString().split('T')[0];
  }
}
