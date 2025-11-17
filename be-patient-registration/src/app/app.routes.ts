// // // src/app/app.routes.ts
// // import { Routes } from '@angular/router';

// // export const routes: Routes = [
// //   {
// //     path: '',
// //     loadChildren: () =>
// //       import('./pages/patients/patients-module').then(m => m.PatientsModule)
// //   },
// //   { path: '**', redirectTo: '' }
// // ];

// // src/app/app.routes.ts
// import { Routes } from '@angular/router';

// export const routes: Routes = [
  // {
    // path: '',
    // redirectTo: 'patients',
    // pathMatch: 'full'
  // },
  // {
    // path: 'patients',
    // loadChildren: () =>
      // import('./pages/patients/patients-module').then(m => m.PatientsModule),
  // },
// //   {
// //     path: 'health-plans',
// //     loadChildren: () =>
// //       import('./pages/health-plans/health-plans-module').then(m => m.HealthPlansModule),
// //   },
  // { path: '**', redirectTo: 'patients' }
// ];

import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./pages/home/home-module').then(m => m.HomeModule),
  },
  {
    path: 'patients',
    loadChildren: () =>
      import('./pages/patients/patients-module').then(m => m.PatientsModule),
  },
  {
    path: 'health-plans',
    loadChildren: () =>
      import('./pages/health-plans/health-plans-module').then(m => m.HealthPlansModule),
  },
  { path: '**', redirectTo: '' },
];
