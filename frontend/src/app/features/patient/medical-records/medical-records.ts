import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MedicalRecordService } from '../../../core/services/medical-record';
import { AuthService } from '../../../core/services/auth';
import { MedicalRecordDto } from '../../../core/models/medical-record.models';

@Component({
  selector: 'app-medical-records',
  imports: [CommonModule, MatProgressSpinnerModule, MatIconModule],
  templateUrl: './medical-records.html',
  styleUrl: './medical-records.scss',
})
export class MedicalRecords implements OnInit {
  records: MedicalRecordDto[] = [];
  loading = false;

  constructor(
    private medicalRecordService: MedicalRecordService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.loading = true;
    const patientId = this.authService.getPatientId();
    this.medicalRecordService.getByPatientId(patientId).subscribe({
      next: (data) => {
        this.records = data;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }
}
