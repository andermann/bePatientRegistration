// src/app/features/patients/pages/patient-form/patient-form.component.ts
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

    console.log('baseRequest', baseRequest);


    if (this.isEditMode && this.id) {
      // const request: UpdatePatientRequest = {
      //   ...baseRequest,
      //   isActive: raw.isActive!
      // };
      const request: UpdatePatientRequest = baseRequest;

      console.log('baseRequest', baseRequest);

      this.patientsService.update(this.id, request).subscribe({
        next: () => this.router.navigate(['/pacientes']),
        // error: (err) => {
        //   console.error('Erro ao atualizar paciente', err);
        //   this.error = 'Erro ao salvar paciente.';
        //   this.loading = false;
        // }
        error: (err) => this.handleApiError(err)
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
        error: (err) => this.handleApiError(err)
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
    Object.entries(this.form.controls).forEach(([key, control]) => {
      if (control.invalid) {
        console.log(key, control.errors);
      }
    });
    console.log('Erros do form (nível grupo):', this.form.errors);
    console.groupEnd();
  }

  private handleApiError(err: any): void {
      console.error('Erro ao salvar paciente', err);
      console.error('Erro detalhado da API:', err?.error);

      this.loading = false;
      this.validationErrors = [];
      this.error = undefined;

      const problem = err?.error;
      if (!problem) {
        this.error = 'Erro ao salvar paciente. Tente novamente.';
        return;
      }

      // Caso padrão do .NET: ProblemDetails com "errors"
      if (problem.errors && typeof problem.errors === 'object') {
        const msgs: string[] = [];

        for (const key of Object.keys(problem.errors)) {
          const value = problem.errors[key];
          if (Array.isArray(value)) {
            value.forEach((m: string) => msgs.push(m));
          } else if (typeof value === 'string') {
            msgs.push(value);
          }

          // Opcional: associar erro do backend a um campo específico
          if (key === 'Gender') {
            this.form.get('gender')?.setErrors({ api: true });
          }
          if (key === 'HealthPlanId') {
            this.form.get('healthPlanId')?.setErrors({ api: true });
          }
          // ... se quiser mapear outros campos
        }

        if (msgs.length) {
          this.validationErrors = msgs; // 👈 vai aparecer no alerta amarelo
          return;
        }
      }

      // Fallback: usa o título do ProblemDetails, se existir
      if (problem.title) {
        this.error = problem.title; // 👈 vai aparecer no alerta vermelho
        return;
      }

      // Último fallback
      this.error = 'Erro ao salvar paciente. Verifique os dados informados.';
    }

  private handleApiErrorXXX(err: any): void {
      console.error('Erro ao salvar paciente', err);
      console.error('Erro detalhado da API:', err?.error);

      this.loading = false;
      this.validationErrors = [];
      this.error = undefined;

      const problem = err?.error;
      if (!problem) {
        this.error = 'Erro ao salvar paciente. Tente novamente.';
        return;
      }

      // Mostra o título (ex: "One or more validation errors occurred.")
      if (problem.title) {
        this.error = problem.title;
      }

      if (problem.errors && typeof problem.errors === 'object') {
        const msgs: string[] = [];

        for (const key of Object.keys(problem.errors)) {
          const value = problem.errors[key];
          const textos = Array.isArray(value) ? value : [value];

          textos.forEach((m: string) => {
            // 🔹 Campos com mensagem “amigável” opcional
            if (key === 'HealthPlanId') {
              msgs.push('Convênio é obrigatório.');
              this.form.get('healthPlanId')?.setErrors({ api: true });
            } else if (key === 'HealthPlanCardNumber') {
              msgs.push('Nº carteirinha é obrigatório.');
              this.form.get('healthPlanCardNumber')?.setErrors({ api: true });
            } else {
              // 🔹 Qualquer outro campo usa a mensagem que veio da API
              // aqui entra "Ano de validade não pode ser menor que o ano atual."
              msgs.push(m);
            }
          });
        }

        this.validationErrors = msgs;
        return;
      }

      // fallback se não houver "errors"
      if (!this.error) {
        this.error = 'Erro ao salvar paciente. Verifique os dados informados.';
      }
    }


  cancel(): void {
    this.router.navigate(['/pacientes']);
  }
}
