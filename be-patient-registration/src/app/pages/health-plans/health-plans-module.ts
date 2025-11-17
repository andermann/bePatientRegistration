// src/app/pages/health-plans/health-plans-module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HealthPlansListComponent } from './health-plans-list.component';

const routes: Routes = [
  { path: '', component: HealthPlansListComponent }
];

@NgModule({
  declarations: [HealthPlansListComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class HealthPlansModule {}
