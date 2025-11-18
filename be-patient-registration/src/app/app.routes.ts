import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { PatientsListComponent } from './features/patients/pages/patients-list/patients-list.component';
import { PatientFormComponent } from './features/patients/pages/patient-form/patient-form.component';
import { HealthPlansListComponent } from './features/health-plans/pages/health-plans-list/health-plans-list.component';
import { HealthPlanFormComponent } from './features/health-plans/pages/health-plan-form/health-plan-form.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },

  { path: 'home', component: HomeComponent },

  { path: 'pacientes', component: PatientsListComponent },
  { path: 'pacientes/novo', component: PatientFormComponent },
  { path: 'pacientes/:id', component: PatientFormComponent },

  { path: 'convenios', component: HealthPlansListComponent },
  { path: 'convenios/novo', component: HealthPlanFormComponent },
  { path: 'convenios/:id', component: HealthPlanFormComponent },

  { path: '**', redirectTo: 'home' }
];
