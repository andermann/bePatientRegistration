// // // // src/app/app.routes.ts
// // // import { Routes } from '@angular/router';

// // // export const routes: Routes = [
// // //   {
// // //     path: '',
// // //     loadChildren: () =>
// // //       import('./pages/patients/patients-module').then(m => m.PatientsModule)
// // //   },
// // //   { path: '**', redirectTo: '' }
// // // ];

// // // src/app/app.routes.ts
// // import { Routes } from '@angular/router';

// // export const routes: Routes = [
//   // {
//     // path: '',
//     // redirectTo: 'patients',
//     // pathMatch: 'full'
//   // },
//   // {
//     // path: 'patients',
//     // loadChildren: () =>
//       // import('./pages/patients/patients-module').then(m => m.PatientsModule),
//   // },
// // //   {
// // //     path: 'health-plans',
// // //     loadChildren: () =>
// // //       import('./pages/health-plans/health-plans-module').then(m => m.HealthPlansModule),
// // //   },
//   // { path: '**', redirectTo: 'patients' }
// // ];

// import { Routes } from '@angular/router';
// import { HealthPlansListComponent } from './pages/health-plans/health-plans-list.component';

// export const routes: Routes = [
//   {
//     path: '',
//     loadChildren: () =>
//       import('./pages/home/home-module').then(m => m.HomeModule),
//   },
//   {
//     path: 'patients',
//     loadChildren: () =>
//       import('./pages/patients/patients-module').then(m => m.PatientsModule),
//   },
//   // {
//   //   path: 'health-plans',
//   //   loadChildren: () =>
//   //     import('./pages/health-plans/health-plans-module').then(m => m.HealthPlansModule),
//   // },
//   { path: 'health-plans', component: HealthPlansListComponent },

//   { path: '**', redirectTo: '' },
// ];

import { Routes } from '@angular/router';

import { PatientListComponent } from './pages/patients/patient-list/patient-list.component';
import { PatientFormComponent } from './pages/patients/patient-form/patient-form.component';

import { HealthPlansListComponent } from './pages/health-plans/health-plans-list.component';

export const routes: Routes = [
  { path: '', redirectTo: 'patients', pathMatch: 'full' },

  { path: 'patients', component: PatientListComponent },
  { path: 'patients/new', component: PatientFormComponent },
  { path: 'patients/:id', component: PatientFormComponent },

  { path: 'health-plans', component: HealthPlansListComponent },
  
];
