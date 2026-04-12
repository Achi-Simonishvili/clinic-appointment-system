import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;
  private readonly tokenKey = 'auth_token';
  private readonly doctorIdKey = 'doctor_id';
  private readonly patientIdKey = 'patient_id';

  constructor(
    private http: HttpClient,
    private router: Router,
  ) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap((response) => {
        this.storeToken(response.token);
        if (response.role === 'Doctor') {
          this.fetchAndStoreDoctorId();
        }
        if (response.role === 'Patient') {
          this.fetchAndStorePatientId();
        }
      }),
    );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, request)
      .pipe(tap((response) => this.storeToken(response.token)));
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.doctorIdKey);
    localStorage.removeItem(this.patientIdKey);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getRole(): string | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null;
    } catch {
      return null;
    }
  }

  getDoctorId(): string {
    return localStorage.getItem(this.doctorIdKey) ?? '';
  }

  private fetchAndStoreDoctorId(): void {
    this.http.get<{ id: string }>(`${environment.apiUrl}/doctors/me`).subscribe({
      next: (doctor) => localStorage.setItem(this.doctorIdKey, doctor.id),
    });
  }

  getPatientId(): string {
    return localStorage.getItem(this.patientIdKey) ?? '';
  }

  private fetchAndStorePatientId(): void {
    this.http.get<{ id: string }>(`${environment.apiUrl}/patients/me`).subscribe({
      next: (patient) => localStorage.setItem(this.patientIdKey, patient.id),
    });
  }

  private storeToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }
}
