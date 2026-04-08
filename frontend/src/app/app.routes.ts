import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full',
  },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then((m) => m.authRoutes),
  },
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.routes').then((m) => m.adminRoutes),
  },
  {
    path: 'doctor',
    loadChildren: () => import('./features/doctor/doctor.routes').then((m) => m.doctorRoutes),
  },
  {
    path: 'patient',
    loadChildren: () => import('./features/patient/patient.routes').then((m) => m.patientRoutes),
  },
  {
    path: '**',
    redirectTo: 'auth/login',
  },
];
