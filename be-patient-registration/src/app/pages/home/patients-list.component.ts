import { Component, OnInit } from '@angular/core';
import { NgIf, NgFor, NgClass } from '@angular/common';
import { Router } from '@angular/router';

import { PatientsService } from './../../features/patients/services/patients.service';
import { Patient } from './../../core/models/patient.model';

@Component({
  selector: 'app-patients-list',
  standalone: true,
  imports: [NgIf, NgFor, NgClass], // 👈 NgClass aqui!
  templateUrl: './patients-list.component.html'
})
export class PatientsListComponent implements OnInit {
  patients: Patient[] = [];
  loading = true;
  error?: string;

  constructor(
    private patientsService: PatientsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = undefined;

    this.patientsService.getAll().subscribe({
      next: data => {
        this.patients = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Erro ao carregar pacientes.';
        this.loading = false;
      }
    });
  }

  newPatient(): void {
    this.router.navigate(['/pacientes/novo']);
  }

  editPatient(patient: Patient): void {
    this.router.navigate(['/pacientes', patient.id]);
  }

  deletePatient(patient: Patient): void {
    if (!confirm(`Deseja inativar o paciente "${patient.fullName}"?`)) {
      return;
    }

    this.patientsService.delete(patient.id).subscribe({
      next: () => this.load(),
      error: () => alert('Erro ao inativar paciente.')
    });
  }
}
