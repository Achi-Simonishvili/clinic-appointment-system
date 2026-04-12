import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DoctorDto, DoctorFilterRequest } from '../models/doctor.models';

@Injectable({ providedIn: 'root' })
export class DoctorService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAll(filter?: DoctorFilterRequest): Observable<any> {
    let params = new HttpParams();
    if (filter?.search) params = params.set('search', filter.search);
    if (filter?.specialization) params = params.set('specialization', filter.specialization);
    if (filter?.pageNumber) params = params.set('pageNumber', filter.pageNumber);
    if (filter?.pageSize) params = params.set('pageSize', filter.pageSize);
    return this.http.get<any>(`${this.apiUrl}/doctors`, { params });
  }

  getById(id: string): Observable<DoctorDto> {
    return this.http.get<DoctorDto>(`${this.apiUrl}/doctors/${id}`);
  }
}
