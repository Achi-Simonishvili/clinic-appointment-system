import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { DoctorDto } from '../../../core/models/doctor.models';
import { StepDoctor } from './step-doctor/step-doctor';
import { StepSlot } from './step-slot/step-slot';
import { StepConfirm } from './step-confirm/step-confirm';

@Component({
  selector: 'app-booking',
  imports: [CommonModule, StepDoctor, StepSlot, StepConfirm],
  templateUrl: './booking.html',
  styleUrl: './booking.scss',
})
export class Booking {
  currentStep = 0;
  selectedDoctor: DoctorDto | null = null;
  selectedDate = '';
  selectedSlot = '';

  steps = ['Choose doctor', 'Choose slot', 'Confirm'];

  onDoctorSelected(doctor: DoctorDto): void {
    this.selectedDoctor = doctor;
    this.currentStep = 1;
  }

  onSlotSelected(data: { date: string; slot: string }): void {
    this.selectedDate = data.date;
    this.selectedSlot = data.slot;
    this.currentStep = 2;
  }

  onConfirmed(): void {
    this.currentStep = 3;
  }

  goBack(): void {
    if (this.currentStep > 0) this.currentStep--;
  }
}
