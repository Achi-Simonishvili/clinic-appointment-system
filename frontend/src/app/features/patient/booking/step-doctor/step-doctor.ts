import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { DoctorService } from '../../../../core/services/doctor';
import { DoctorDto } from '../../../../core/models/doctor.models';

@Component({
  selector: 'app-step-doctor',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
  ],
  templateUrl: './step-doctor.html',
  styleUrl: './step-doctor.scss',
})
export class StepDoctor implements OnInit {
  @Output() doctorSelected = new EventEmitter<DoctorDto>();

  doctors: DoctorDto[] = [];
  loading = false;
  searchControl = new FormControl('');

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.loadDoctors();
    this.searchControl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe((search) => this.loadDoctors(search ?? ''));
  }

  loadDoctors(search = ''): void {
    this.loading = true;
    this.doctorService.getAll({ search, pageSize: 20 }).subscribe({
      next: (res) => {
        this.doctors = res.items ?? res;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  select(doctor: DoctorDto): void {
    this.doctorSelected.emit(doctor);
  }
}
