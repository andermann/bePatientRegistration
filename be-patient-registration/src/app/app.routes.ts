// src/app/app.routes.ts
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./pages/patients/patients-module').then(m => m.PatientsModule)
  },
  { path: '**', redirectTo: '' }
];