import { Routes } from '@angular/router';

export const doctorRoutes: Routes = [
  {
    path: 'availability',
    loadComponent: () => import('./availability/availability').then((m) => m.Availability),
  },
  {
    path: 'appointments',
    loadComponent: () => import('./appointments/appointments').then((m) => m.Appointments),
  },
  {
    path: 'medical-records',
    loadComponent: () => import('./medical-records/medical-records').then((m) => m.MedicalRecords),
  },
  {
    path: '',
    redirectTo: 'appointments',
    pathMatch: 'full',
  },
];
