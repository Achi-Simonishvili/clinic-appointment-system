import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { roleGuard } from './core/guards/role-guard';

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
    canActivate: [authGuard, roleGuard],
    data: { role: 'Admin' },
    loadChildren: () => import('./features/admin/admin.routes').then((m) => m.adminRoutes),
  },
  {
    path: 'doctor',
    canActivate: [authGuard, roleGuard],
    data: { role: 'Doctor' },
    loadChildren: () => import('./features/doctor/doctor.routes').then((m) => m.doctorRoutes),
  },
  {
    path: 'patient',
    canActivate: [authGuard, roleGuard],
    data: { role: 'Patient' },
    loadChildren: () => import('./features/patient/patient.routes').then((m) => m.patientRoutes),
  },
  {
    path: '**',
    redirectTo: 'auth/login',
  },
];
