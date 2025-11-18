import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxMaskDirective } from 'ngx-mask';

import {
  Gender,
  Uf,
  CreatePatientRequest,
  UpdatePatientRequest,
  Patient
} from '../../core/models/patient.model';
// import { PatientsService } from '../../services/patients.service';
import { PatientsService } from './../../features/patients/services/patients.service';
// import { HealthPlansService } from '../../../health-plans/services/health-plans.service';
import { HealthPlansService } from '../../features/health-plans/services/health-plans.service';
import { HealthPlan } from './../../core/models/health-plan.model';
import { CustomValidators } from './../../core/validators/custom-validators';

@Component({
  selector: 'app-patient-form',
  standalone: true,
  imports: [NgIf, NgFor, ReactiveFormsModule, NgxMaskDirective],
  templateUrl: './patient-form.component.html'
})
export class PatientFormComponent implements OnInit {
  // FormBuilder injetado como propriedade
  private fb = inject(FormBuilder);

  form = this.fb.nonNullable.group(
    {
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      dateOfBirth: ['', [Validators.required, CustomValidators.notFutureDate()]],
      gender: [Gender.Male, [Validators.required]],
      cpf: [''],
      rg: ['', [Validators.required, Validators.maxLength(20)]],
      ufRg: [Uf.RJ, [Validators.required]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
      mobilePhone: ['', [Validators.required, Validators.maxLength(20)]],
      landlinePhone: [''],
      healthPlanId: ['', [Validators.required]],
      healthPlanCardNumber: ['', [Validators.required, Validators.maxLength(50)]],
      healthPlanCardExpirationMonth: [1, [Validators.required, Validators.min(1), Validators.max(12)]],
      healthPlanCardExpirationYear: [2024, [Validators.required, Validators.min(2024), Validators.max(2100)]],
      isActive: [true]
    },
    {
      validators: [
        CustomValidators.atLeastOnePhone('mobilePhone', 'landlinePhone')
      ]
    }
  );

  genders = [
    { value: Gender.Male, label: 'Masculino' },
    { value: Gender.Female, label: 'Feminino' },
    { value: Gender.Other, label: 'Outro' }
  ];

  ufs = Object.keys(Uf).filter(k => isNaN(Number(k)));

  healthPlans: HealthPlan[] = [];
  loading = false;
  isEditMode = false;
  id?: string;
  error?: string;

  constructor(
    private patientsService: PatientsService,
    private healthPlansService: HealthPlansService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') || undefined;
    this.isEditMode = !!this.id;

    this.loadHealthPlans();

    if (this.isEditMode && this.id) {
      this.loadPatient(this.id);
    }
  }

  get f() {
    return this.form.controls;
  }

  get groupErrors() {
    return this.form.errors;
  }

  private loadHealthPlans(): void {
    this.healthPlansService.getAll().subscribe({
      next: plans => (this.healthPlans = plans),
      error: () => (this.error = 'Erro ao carregar convênios.')
    });
  }

  private loadPatient(id: string): void {
    this.loading = true;
    this.patientsService.getById(id).subscribe({
      next: (patient: Patient) => {
        this.form.patchValue({
          firstName: patient.firstName,
          lastName: patient.lastName,
          dateOfBirth: patient.dateOfBirth.substring(0, 10),
          gender: patient.gender,
          cpf: patient.cpf ?? '',
          rg: patient.rg,
          ufRg: patient.ufRg,
          email: patient.email,
          mobilePhone: patient.mobilePhone,
          landlinePhone: patient.landlinePhone ?? '',
          healthPlanId: patient.healthPlanId,
          healthPlanCardNumber: patient.healthPlanCardNumber,
          healthPlanCardExpirationMonth: patient.healthPlanCardExpirationMonth,
          healthPlanCardExpirationYear: patient.healthPlanCardExpirationYear,
          isActive: patient.isActive ?? true
        });
        this.loading = false;
      },
      error: () => {
        this.error = 'Paciente não encontrado.';
        this.loading = false;
      }
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();

    const baseRequest: CreatePatientRequest = {
      firstName: raw.firstName!,
      lastName: raw.lastName!,
      dateOfBirth: raw.dateOfBirth!,
      gender: raw.gender!,
      cpf: raw.cpf || null,
      rg: raw.rg!,
      ufRg: raw.ufRg!,
      email: raw.email!,
      mobilePhone: raw.mobilePhone!,
      landlinePhone: raw.landlinePhone || null,
      healthPlanId: raw.healthPlanId!,
      healthPlanCardNumber: raw.healthPlanCardNumber!,
      healthPlanCardExpirationMonth: raw.healthPlanCardExpirationMonth!,
      healthPlanCardExpirationYear: raw.healthPlanCardExpirationYear!
      , isActive: raw.isActive
    };

    this.loading = true;

    if (this.isEditMode && this.id) {
      const request: UpdatePatientRequest = {
        ...baseRequest,
        isActive: raw.isActive!
      };

      this.patientsService.update(this.id, request).subscribe({
        next: () => this.router.navigate(['/pacientes']),
        error: () => {
          this.error = 'Erro ao salvar paciente.';
          this.loading = false;
        }
      });
    } else {
      this.patientsService.create(baseRequest).subscribe({
        next: () => this.router.navigate(['/pacientes']),
        error: () => {
          this.error = 'Erro ao salvar paciente.';
          this.loading = false;
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/pacientes']);
  }
}
