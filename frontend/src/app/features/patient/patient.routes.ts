import { Routes } from '@angular/router';

export const patientRoutes: Routes = [
  {
    path: 'booking',
    loadComponent: () => import('./booking/booking').then((m) => m.Booking),
  },
  {
    path: '',
    redirectTo: 'booking',
    pathMatch: 'full',
  },
];
