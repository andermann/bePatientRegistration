// src/app/pages/patients/patients-routing-module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientListComponent } from './patient-list/patient-list.component';
import { PatientFormComponent } from './patient-form/patient-form.component';

const routes: Routes = [
  { path: '', component: PatientListComponent },
  { path: 'new', component: PatientFormComponent },
  { path: ':id', component: PatientFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientsRoutingModule {}
