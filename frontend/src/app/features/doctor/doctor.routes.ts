import { Routes } from '@angular/router';

export const doctorRoutes: Routes = [
  {
    path: 'availability',
    loadComponent: () => import('./availability/availability').then((m) => m.Availability),
  },
  {
    path: '',
    redirectTo: 'availability',
    pathMatch: 'full',
  },
];
