import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  MedicalRecordDto,
  CreateMedicalRecordRequest,
  UpdateMedicalRecordRequest,
} from '../models/medical-record.models';

@Injectable({ providedIn: 'root' })
export class MedicalRecordService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getByPatientId(patientId: string): Observable<MedicalRecordDto[]> {
    return this.http.get<MedicalRecordDto[]>(`${this.apiUrl}/medicalrecords/patient/${patientId}`);
  }

  getByDoctorId(doctorId: string): Observable<MedicalRecordDto[]> {
    return this.http.get<MedicalRecordDto[]>(`${this.apiUrl}/medicalrecords/doctor/${doctorId}`);
  }

  create(doctorId: string, request: CreateMedicalRecordRequest): Observable<MedicalRecordDto> {
    return this.http.post<MedicalRecordDto>(
      `${this.apiUrl}/medicalrecords?doctorId=${doctorId}`,
      request,
    );
  }

  update(id: string, request: UpdateMedicalRecordRequest): Observable<MedicalRecordDto> {
    return this.http.put<MedicalRecordDto>(`${this.apiUrl}/medicalrecords/${id}`, request);
  }
}
