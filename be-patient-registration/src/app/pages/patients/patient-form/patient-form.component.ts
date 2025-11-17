import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';

import { HealthPlanService } from '../../../services/health-plan.service';
import { PatientService } from '../../../services/patient.service';

import { HealthPlan } from '../../../models/health-plan.model';
import { Patient } from '../../../models/patient.model';

@Component({
  selector: 'app-patient-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule
  ],
  templateUrl: './patient-form.component.html',
  styleUrls: ['./patient-form.component.scss']
})
export class PatientFormComponent implements OnInit {

  form!: FormGroup;
  saving = false;
  error?: string;

  healthPlans: HealthPlan[] = [];
  editingId?: string;

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService,
    private healthPlanService: HealthPlanService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadHealthPlans();

    this.editingId = this.route.snapshot.paramMap.get('id') ?? undefined;

    if (this.editingId) {
      this.loadPatient(this.editingId);
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      dateOfBirth: ['', Validators.required],
      gender: [1, Validators.required],
      cpf: [''],
      rg: [''],
      ufRg: [''],
      email: ['', [Validators.email]],
      mobilePhone: [''],
      landlinePhone: [''],

      healthPlanId: [null, Validators.required],
      healthPlanCardNumber: [''],
      healthPlanCardExpirationMonth: [''],
      healthPlanCardExpirationYear: ['']
    });
  }

  loadHealthPlans(): void {
    this.healthPlanService.getAll().subscribe({
      next: plans => this.healthPlans = plans,
      error: () => this.error = 'Erro ao carregar convênios.'
    });
  }

  loadPatient(id: string): void {
    this.patientService.getById(id).subscribe({
      next: p => this.form.patchValue(p),
      error: () => this.error = 'Erro ao carregar paciente.'
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    const dto = this.form.value;

    const request = this.editingId
      ? this.patientService.update(this.editingId, dto)
      : this.patientService.create(dto);

    request.subscribe({
      next: () => this.router.navigate(['/patients']),
      error: err => {
        console.error(err);
        this.error = 'Erro ao salvar.';
        this.saving = false;
      }
    });
  }
}
