import { Component, OnInit } from '@angular/core';
import { PatientsService } from '../../features/patients/services/patients.service';
import { HealthPlansService } from '../../features/health-plans/services/health-plans.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NgIf],
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

  loading = true;
  error?: string;

  totalPatients = 0;
  totalHealthPlans = 0;

  constructor(
    private patientsService: PatientsService,
    private healthPlansService: HealthPlansService
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.loading = true;

    Promise.all([
      this.patientsService.getAll().toPromise(),
      this.healthPlansService.getAll().toPromise()
    ])
    .then(([patients, plans]) => {
      this.totalPatients = patients?.length ?? 0;
      this.totalHealthPlans = plans?.length ?? 0;
      this.loading = false;
    })
    .catch(err => {
      console.error('Erro ao carregar dashboard', err);
      this.error = 'Erro ao obter informações do dashboard.';
      this.loading = false;
    });
  }
}
