import { Routes } from '@angular/router';

export const adminRoutes: Routes = [
  {
    path: 'appointments',
    loadComponent: () => import('./appointments/appointments').then((m) => m.Appointments),
  },
  {
    path: '',
    redirectTo: 'appointments',
    pathMatch: 'full',
  },
];
