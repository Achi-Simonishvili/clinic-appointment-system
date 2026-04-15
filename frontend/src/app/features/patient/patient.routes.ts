import { Routes } from '@angular/router';

export const patientRoutes: Routes = [
  {
    path: 'booking',
    loadComponent: () => import('./booking/booking').then((m) => m.Booking),
  },
  {
    path: 'medical-records',
    loadComponent: () => import('./medical-records/medical-records').then((m) => m.MedicalRecords),
  },
  {
    path: '',
    redirectTo: 'booking',
    pathMatch: 'full',
  },
];
