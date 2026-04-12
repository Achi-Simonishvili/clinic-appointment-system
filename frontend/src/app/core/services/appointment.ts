import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AppointmentDto, BookAppointmentRequest } from '../models/appointment.models';

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  book(patientId: string, request: BookAppointmentRequest): Observable<AppointmentDto> {
    return this.http.post<AppointmentDto>(
      `${this.apiUrl}/appointments?patientId=${patientId}`,
      request,
    );
  }

  getByPatientId(patientId: string): Observable<AppointmentDto[]> {
    return this.http.get<AppointmentDto[]>(`${this.apiUrl}/appointments/patient/${patientId}`);
  }

  getByDoctorId(doctorId: string): Observable<AppointmentDto[]> {
    return this.http.get<AppointmentDto[]>(`${this.apiUrl}/appointments/doctor/${doctorId}`);
  }

  getAll(params?: any): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/appointments`, { params });
  }

  updateStatus(id: string, status: string): Observable<AppointmentDto> {
    return this.http.patch<AppointmentDto>(`${this.apiUrl}/appointments/${id}/status`, { status });
  }
}
