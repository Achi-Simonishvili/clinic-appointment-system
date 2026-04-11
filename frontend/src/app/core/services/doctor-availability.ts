import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DoctorAvailabilityDto, SetAvailabilityRequest } from '../models/availability.models';

@Injectable({ providedIn: 'root' })
export class DoctorAvailabilityService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getByDoctorId(doctorId: string): Observable<DoctorAvailabilityDto[]> {
    return this.http.get<DoctorAvailabilityDto[]>(
      `${this.apiUrl}/doctors/${doctorId}/availability`,
    );
  }

  setAvailability(
    doctorId: string,
    request: SetAvailabilityRequest,
  ): Observable<DoctorAvailabilityDto> {
    return this.http.post<DoctorAvailabilityDto>(
      `${this.apiUrl}/doctors/${doctorId}/availability`,
      request,
    );
  }

  delete(doctorId: string, id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/doctors/${doctorId}/availability/${id}`);
  }
}
