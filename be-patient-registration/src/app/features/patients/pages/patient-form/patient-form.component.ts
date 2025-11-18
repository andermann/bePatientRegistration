// src/app/features/patients/pages/patient-form/patient-form.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators,AbstractControl,ValidationErrors} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxMaskDirective } from 'ngx-mask';

import {
  Gender,
  Uf,
  CreatePatientRequest,
  UpdatePatientRequest,
  Patient
} from '../../../../core/models/patient.model';
import { PatientsService } from '../../services/patients.service';
import { HealthPlansService } from '../../../health-plans/services/health-plans.service';
import { HealthPlan } from '../../../../core/models/health-plan.model';
import { CustomValidators } from '../../../../core/validators/custom-validators';


@Component({
  selector: 'app-patient-form',
  standalone: true,
  imports: [NgIf, NgFor, ReactiveFormsModule, NgxMaskDirective],
  templateUrl: 'patient-form.component.html'
})
export class PatientFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  validationErrors: string[] = [];
  submitted = false;

  form = this.fb.nonNullable.group(
    {
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      dateOfBirth: ['', [Validators.required, CustomValidators.notFutureDate()]],
      gender: [Gender.Male, [Validators.required]],
      cpf: [''],
      rg: ['', [Validators.required, Validators.maxLength(20)]],
      // ufRg: [Uf.RJ, [Validators.required]],
      ufRg: [0 as number, [Validators.required, Validators.min(1)]],
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
    { value: 1, label: 'Masculino' },
    { value: 2, label: 'Feminino' },
    { value: 3, label: 'Outro' }
  ];

  // ufs = Object.keys(Uf).filter(k => isNaN(Number(k)));
  ufs: { id: number; code: string }[] = Object.keys(Uf)
      .filter(key => isNaN(Number(key))) // pega só os nomes (AC, AL, AP...)
      .map(key => ({
        id: (Uf as any)[key] as number, // valor numérico do enum
        code: key     as string                   // texto que vamos mostrar no select
      }));

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

  // usado no template como f.firstName, f.email etc
  get f() {
    return this.form.controls;
  }

  // usado no template como groupErrors?.phoneRequired
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
      console.log('ank deu ruim - form inválido');

      // Força os campos a ficarem "touched" para aparecerem os erros individuais
      this.form.markAllAsTouched();

      // Monta a lista de erros em tela
      this.buildValidationErrors();

      // Log extra para você debugar no console
      this.logInvalidControls();

      return;
    }

    const raw = this.form.getRawValue();
    console.log('ANK raw', raw);

    // Normaliza campos com máscara (remove pontos, traços, parênteses etc.)
    const onlyDigits = (value?: string | null) =>
      (value || '').replace(/\D/g, '') || null;

    const baseRequest: CreatePatientRequest = {
      firstName: raw.firstName!.trim(),
      lastName: raw.lastName!.trim(),
      dateOfBirth: raw.dateOfBirth!, // já vem no formato ISO (yyyy-MM-dd)
      gender: raw.gender!,
      // CPF é opcional, mas sempre enviado só com dígitos
      cpf: raw.cpf ? onlyDigits(raw.cpf) : null,
      // RG e telefones só com dígitos
      rg: onlyDigits(raw.rg!)!,
      ufRg: raw.ufRg!,
      email: raw.email!.trim(),
      mobilePhone: onlyDigits(raw.mobilePhone!)!,
      landlinePhone: raw.landlinePhone ? onlyDigits(raw.landlinePhone) : null,
      healthPlanId: raw.healthPlanId!,
      healthPlanCardNumber: raw.healthPlanCardNumber!.trim(),
      healthPlanCardExpirationMonth: raw.healthPlanCardExpirationMonth!,
      healthPlanCardExpirationYear: raw.healthPlanCardExpirationYear!
      // isActive: this.isEditMode ? raw.isActive : true
      , isActive: raw.isActive ? raw.isActive : true
    };

    this.error = undefined;
    this.loading = true;

    if (this.isEditMode && this.id) {
      // const request: UpdatePatientRequest = {
      //   ...baseRequest,
      //   isActive: raw.isActive!
      // };
      const request: UpdatePatientRequest = baseRequest;

      this.patientsService.update(this.id, request).subscribe({
        next: () => this.router.navigate(['/pacientes']),
        // error: (err) => {
        //   console.error('Erro ao atualizar paciente', err);
        //   this.error = 'Erro ao salvar paciente.';
        //   this.loading = false;
        // }
        error: (err) => {
          console.error('Erro detalhado da API:', err.error); 
          this.error = 'Erro ao salvar paciente.';
          this.error = this.extrairMensagemErro(err);
          this.handleApiError(err)
        }
      });
    } else {
      this.patientsService.create(baseRequest).subscribe({
        next: () => this.router.navigate(['/pacientes']),
        // error: (err) => {
        //   console.error('Erro ao salvar paciente', err);
        //   console.error('Erro detalhado da API:', err.error); 
        //   this.error = 'Erro ao salvar paciente.';
        //   this.error = this.extrairMensagemErro(err);
        //   this.loading = false;
        // }
        error: (err) => {
          console.error('Erro detalhado da API:', err.error); 
          this.error = 'Erro ao salvar paciente.';
          this.error = this.extrairMensagemErro(err);
          this.handleApiError(err)
        }
      });
    }
    
  }

  private extrairMensagemErro(err: any): string {
    // Quando a API usa ProblemDetails (padrão .NET)
    if (err?.error?.title) {
      return err.error.title;
    }

    // Quando a API manda um objeto de validação:
    // { errors: { Campo: [ 'msg1', 'msg2' ] } }
    if (err?.error?.errors) {
      const erros = err.error.errors;
      const mensagens: string[] = [];
      for (const key of Object.keys(erros)) {
        mensagens.push(...erros[key]);
      }
      return mensagens.join(' | ');
    }

    // Fallback
    return 'Erro ao salvar paciente. Verifique os dados informados.';
  }

  private buildValidationErrors(): void {
    const messages: string[] = [];

    const labels: Record<string, string> = {
      firstName: 'Nome',
      lastName: 'Sobrenome',
      dateOfBirth: 'Data de nascimento',
      gender: 'Gênero',
      cpf: 'CPF',
      rg: 'RG',
      ufRg: 'UF do RG',
      email: 'E-mail',
      mobilePhone: 'Celular',
      landlinePhone: 'Telefone fixo',
      healthPlanId: 'Convênio',
      healthPlanCardNumber: 'Nº carteirinha',
      healthPlanCardExpirationMonth: 'Mês de validade',
      healthPlanCardExpirationYear: 'Ano de validade',
      isActive: 'Status'
    };

    Object.entries(this.form.controls).forEach(([key, control]) => {
      if (control.invalid) {
        const friendly = labels[key] ?? key;
        const errors = control.errors ?? {};

        if (errors['required']) {
          messages.push(`${friendly} é obrigatório.`);
        }
        if (errors['maxlength']) {
          messages.push(
            `${friendly} ultrapassa o tamanho máximo (${errors['maxlength'].requiredLength} caracteres).`
          );
        }
        if (errors['email']) {
          messages.push(`${friendly} não é um e-mail válido.`);
        }
        if (errors['min']) {
          messages.push(
            `${friendly} não pode ser menor que ${errors['min'].min}.`
          );
        }
        if (errors['max']) {
          messages.push(
            `${friendly} não pode ser maior que ${errors['max'].max}.`
          );
        }
      }
    });

    // Erros do formulário como um todo (validators de grupo)
    const formErrors = this.form.errors ?? {};
    if (formErrors['phoneRequired']) {
      messages.push(formErrors['phoneRequired']);
    }
    if (formErrors['cpf']) {
      messages.push(formErrors['cpf']);
    }

    this.validationErrors = messages;
  }

  private logInvalidControls(): void {
    console.group('Campos inválidos do formulário de paciente');
    this.validationErrors = [];
    this.error = undefined;
    const messages: string[] = [];

    Object.entries(this.form.controls).forEach(([key, value]) => {
      if (value.invalid) {
        const textos = Array.isArray(value) ? value as string[] : [String(value)];
        //const map = fieldMap[key] ?? null;
        console.log('textos', textos);
        console.log(key, value.errors);
        if(key === 'mobilePhone'){
          messages.push('Preencha o Celular com a quantidade mínima de digitos.');
        }else{
          //messages.push(key);
        }

      }
    });
    console.log('Erros do form (nível grupo):', this.form.errors);
    console.groupEnd();
    
    if (messages.length) {
      // Vai aparecer no alerta amarelo "Revise os campos abaixo"
      this.validationErrors = messages;
      return;
    }
  }

  
  private handleApiError(err: any): void {
      console.error('Erro ao salvar paciente', err);
      console.error('Erro detalhado da API:', err?.error);

      this.loading = false;
      this.validationErrors = [];
      this.error = undefined;

      const problem = err?.error ?? err;
      if (!problem) {
        this.error = 'Erro ao salvar paciente. Tente novamente.';
        return;
      }

      // Mapa: nome que vem da API  ->  nome do formControl + label amigável
      const fieldMap: Record<string, { control: string; label: string }> = {
        FirstName: { control: 'firstName', label: 'Nome' },
        LastName: { control: 'lastName', label: 'Sobrenome' },
        DateOfBirth: { control: 'dateOfBirth', label: 'Data de nascimento' },
        Gender: { control: 'gender', label: 'Gênero' },
        Cpf: { control: 'cpf', label: 'CPF' },
        Rg: { control: 'rg', label: 'RG' },
        UfRg: { control: 'ufRg', label: 'UF do RG' },
        Email: { control: 'email', label: 'E-mail' },
        MobilePhone: { control: 'mobilePhone', label: 'Celular' },
        LandlinePhone: { control: 'landlinePhone', label: 'Telefone fixo' },
        HealthPlanId: { control: 'healthPlanId', label: 'Convênio' },
        HealthPlanCardNumber: { control: 'healthPlanCardNumber', label: 'Nº carteirinha' },
        HealthPlanCardExpirationMonth: {
          control: 'healthPlanCardExpirationMonth',
          label: 'Mês de validade'
        },
        HealthPlanCardExpirationYear: {
          control: 'healthPlanCardExpirationYear',
          label: 'Ano de validade'
        }
      };

      const messages: string[] = [];

      // 1) Cenário típico de validação: ProblemDetails com "errors"
      if (problem.errors && typeof problem.errors === 'object') {
        Object.entries(problem.errors).forEach(([key, value]) => {
          const textos = Array.isArray(value) ? value as string[] : [String(value)];
          const map = fieldMap[key] ?? null;

          textos.forEach(m => {
            messages.push(m);

            // Marca o campo correspondente com erro vindo da API
            if (map) {
              const control = this.form.get(map.control);
              if (control) {
                const currentErrors = control.errors || {};
                control.setErrors({ ...currentErrors, api: m });
                control.markAsTouched();
              }
            }
          });
        });

        if (messages.length) {
          // Vai aparecer no alerta amarelo "Revise os campos abaixo"
          this.validationErrors = messages;
          return;
        }
      }

      // 2) DomainException / outros erros sem "errors"
      // (ex: "Convênio informado não existe.", "Já existe um paciente cadastrado com este CPF.")
      if (problem.title && problem.title !== 'One or more validation errors occurred.') {
        this.error = problem.title;
        return;
      }

      if (problem.detail) {
        this.error = problem.detail;
        return;
      }

      if (typeof problem === 'string') {
        this.error = problem;
        return;
      }

      // Fallback final
      this.error = 'Erro ao salvar paciente. Verifique os dados informados.';
  }

  cancel(): void {
    this.router.navigate(['/pacientes']);
  }
}
