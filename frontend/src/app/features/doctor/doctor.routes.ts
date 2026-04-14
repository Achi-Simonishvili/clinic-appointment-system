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
    path: '',
    redirectTo: 'appointments',
    pathMatch: 'full',
  },
];
