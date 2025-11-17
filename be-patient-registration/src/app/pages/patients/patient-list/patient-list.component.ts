// import { CommonModule } from '@angular/common';
// import { RouterModule } from '@angular/router';
// import { Component, OnInit } from '@angular/core';
// import { PatientService } from '../../../services/patient.service';
// import { Patient } from '../../../models/patient.model';

// @Component({
//   selector: 'app-patient-list',
//   standalone: this.patients, // <-- Adicione isso
//   imports: [CommonModule, RouterModule, ReactiveFormsModule], // <-- Adicione as dependências de módulo usadas no template
//   templateUrl: './patient-list.component.html',
//   styleUrls: ['./patient-list.component.scss']
// })
// export class PatientListComponent implements OnInit {

//   patients: Patient[] = [];
//   loading = false;
//   error?: string;

//   constructor(private patientService: PatientService) {}

//   ngOnInit(): void {
//     this.loadPatients();
//   }

//   loadPatients(): void {
//     this.loading = true;
//     this.error = undefined;

//     this.patientService.getAll().subscribe({
//       next: data => {
//         this.patients = data;
// 		console.log('zzz patientService.getAll() data=', data, 'len=', this.patients.length); 
//         this.loading = false;
//       },
//       error: err => {
//         console.error(err);
//         this.error = 'Erro ao carregar pacientes.';
//         this.loading = false;
//       }
//     });
//   }
// }

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { PatientService } from '../../../services/patient.service';
import { Patient } from '../../../models/patient.model';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './patient-list.component.html',
  styleUrls: ['./patient-list.component.scss']
})
export class PatientListComponent implements OnInit {

  patients: Patient[] = [];
  loading = false;
  error?: string;

  constructor(private patientService: PatientService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = undefined;

    this.patientService.getAll().subscribe({
      next: data => {
        console.log("Patients:", data);
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
