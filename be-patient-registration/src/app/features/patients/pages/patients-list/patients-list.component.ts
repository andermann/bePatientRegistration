// src/app/features/patients/pages/patients-list/patients-list.component.ts

import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { PatientsService } from '../../services/patients.service';
import { Patient } from '../../../../core/models/patient.model';

@Component({
  selector: 'app-patients-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './patients-list.component.html'
  // IMPORTANTE: não colocar changeDetection: OnPush aqui por enquanto
})
export class PatientsListComponent implements OnInit {
  patients: Patient[] = [];
  loading = false;
  error?: string;

  constructor(
    private patientsService: PatientsService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = undefined;

    this.patientsService.getAll().subscribe({
      next: (data) => {

        this.patients = data ?? [];
        this.loading = false;

        // força o Angular a atualizar a view
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Erro ao carregar pacientes', err);
        this.error = 'Erro ao carregar pacientes.';
        this.loading = false;

        this.cdr.detectChanges();
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
