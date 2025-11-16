// src/app/pages/patients/patients-module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { PatientsRoutingModule } from './patients-routing-module';
import { PatientListComponent } from './patient-list/patient-list.component';
import { PatientFormComponent } from './patient-form/patient-form.component';

@NgModule({
  declarations: [
    PatientListComponent,
    PatientFormComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    PatientsRoutingModule
  ]
})
export class PatientsModule {}
