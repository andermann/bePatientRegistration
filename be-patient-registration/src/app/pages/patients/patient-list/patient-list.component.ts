import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { PatientService } from '../../../services/patient.service';
import { Patient } from '../../../models/patient.model';


// @Component({
//   selector: 'app-patient-list',
//   templateUrl: './patient-list.component.html',
//   styleUrls: ['./patient-list.component.scss']
// })
@Component({
  selector: 'app-patient-list',
  standalone: false, // <-- Adicione isso
  // imports: [CommonModule, RouterModule], // <-- Adicione as dependências de módulo usadas no template
  templateUrl: './patient-list.component.html',
  styleUrls: ['./patient-list.component.scss']
})
export class PatientListComponent implements OnInit {

  patients: Patient[] = [];
  loading = false;
  error?: string;

  constructor(private patientService: PatientService) {}

  ngOnInit(): void {
    this.loadPatients();
  }

  loadPatients(): void {
    this.loading = true;
    this.error = undefined;

    this.patientService.getAll().subscribe({
      next: data => {
        this.patients = data;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Erro ao carregar pacientes.';
        this.loading = false;
      }
    });
  }
}
