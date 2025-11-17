import { Component, OnInit } from '@angular/core';
// import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../../../services/patient.service';
import { HealthPlanService } from '../../../services/health-plan.service';
import { HealthPlan } from '../../../models/health-plan.model';
import { PatientRequest } from '../../../models/patient.model';

@Component({
  selector: 'app-patient-form',
  standalone: false, // <-- Adicione isso
  templateUrl: './patient-form.component.html',
  styleUrls: ['./patient-form.component.scss']
})
export class PatientFormComponent implements OnInit {

  form!: FormGroup;
  healthPlans: HealthPlan[] = [];
  editingId?: string;
  saving = false;
  error?: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private patientService: PatientService,
    private healthPlanService: HealthPlanService
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadHealthPlans();

    this.editingId = this.route.snapshot.paramMap.get('id') ?? undefined;
    if (this.editingId) {
      // depois você pode implementar carregamento para edição aqui
    }
  }

  // buildForm(): void {
  //   this.form = this.fb.group({
  //     firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
  //     lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
  //     dateOfBirth: [null, [Validators.required]],
  //     gender: [1, [Validators.required]],

  //     cpf: [''],
  //     rg: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
  //     ufRg: ['', [Validators.required]],

  //     email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
  //     mobilePhone: [''],
  //     landlinePhone: [''],

  //     healthPlanId: [null, [Validators.required]],
  //     healthPlanCardNumber: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
  //     healthPlanCardExpirationMonth: [null, [Validators.required, Validators.min(1), Validators.max(12)]],
  //     healthPlanCardExpirationYear: [null, [Validators.required]]
  //   }, {
  //     validators: [this.atLeastOnePhoneValidator()]
  //   });
  // }
  buildForm(): void {
  this.form = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],

    // Data obrigatória + não pode ser futura
    dateOfBirth: [null, [Validators.required, this.dateNotInFutureValidator]],

    gender: [1, [Validators.required]],

    // CPF opcional, mas se preenchido precisa ser válido
    cpf: ['', [this.cpfValidator]],

    rg: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
    ufRg: ['', [Validators.required]],

    email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],

    mobilePhone: [''],
    landlinePhone: [''],

    healthPlanId: [null, [Validators.required]],
    healthPlanCardNumber: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    healthPlanCardExpirationMonth: [null, [Validators.required, Validators.min(1), Validators.max(12)]],
    healthPlanCardExpirationYear: [null, [Validators.required]]
  }, {
    validators: [this.atLeastOnePhoneValidator()]
  });
}



  // // Regra: precisa ter pelo menos um telefone (celular ou fixo)
  // atLeastOnePhoneValidator() {
  //   return (group: FormGroup) => {
  //     const mobile = group.get('mobilePhone')?.value;
  //     const landline = group.get('landlinePhone')?.value;

  //     if (!mobile && !landline) {
  //       const mobileErrors = group.get('mobilePhone')?.errors || {};
  //       group.get('mobilePhone')?.setErrors({ ...mobileErrors, atLeastOnePhone: true });

  //       const landErrors = group.get('landlinePhone')?.errors || {};
  //       group.get('landlinePhone')?.setErrors({ ...landErrors, atLeastOnePhone: true });
  //     } else {
  //       ['mobilePhone', 'landlinePhone'].forEach(name => {
  //         const ctrl = group.get(name);
  //         const errors = ctrl?.errors as { [key: string]: any } | null | undefined;
  //         if (errors && errors['atLeastOnePhone']) {
  //           const { ['atLeastOnePhone']: _, ...rest } = errors;
  //           ctrl!.setErrors(Object.keys(rest).length ? rest : null);
  //         }
  //       });
  //     }
  //     return null;
  //   };
  // }

  // Regra: precisa ter pelo menos um telefone (celular ou fixo)
atLeastOnePhoneValidator() {
  return (group: FormGroup) => {
    const mobile = group.get('mobilePhone')?.value;
    const landline = group.get('landlinePhone')?.value;

    if (!mobile && !landline) {
      const mobileErrors = group.get('mobilePhone')?.errors || {};
      group.get('mobilePhone')?.setErrors({ ...mobileErrors, atLeastOnePhone: true });

      const landErrors = group.get('landlinePhone')?.errors || {};
      group.get('landlinePhone')?.setErrors({ ...landErrors, atLeastOnePhone: true });
    } else {
      ['mobilePhone', 'landlinePhone'].forEach(name => {
        const ctrl = group.get(name);
        const errors = ctrl?.errors as { [key: string]: any } | null | undefined;
        if (errors && errors['atLeastOnePhone']) {
          const { ['atLeastOnePhone']: _, ...rest } = errors;
          ctrl!.setErrors(Object.keys(rest).length ? rest : null);
        }
      });
    }
    return null;
  };
}

//validador de data de nascimento não futura
dateNotInFutureValidator = (control: AbstractControl): ValidationErrors | null => {
  const value = control.value;
  if (!value) return null;

  const date = new Date(value);
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  if (date > today) {
    return { futureDate: true };
  }

  return null;
};


//validador de CPF válido
cpfValidator = (control: AbstractControl): ValidationErrors | null => {
  let cpf: string = (control.value || '').replace(/\D/g, '');
  if (!cpf) {
    // Campo opcional – se vazio, está ok
    return null;
  }

  if (cpf.length !== 11) {
    return { cpfInvalid: true };
  }

  // Rejeita CPFs com todos dígitos iguais tipo 11111111111
  if (/^(\d)\1{10}$/.test(cpf)) {
    return { cpfInvalid: true };
  }

  // Validação dos dígitos verificadores
  const calcCheckDigit = (base: string, factor: number) => {
    let total = 0;
    for (let i = 0; i < base.length; i++, factor--) {
      total += parseInt(base[i], 10) * factor;
    }
    const rest = total % 11;
    return rest < 2 ? 0 : 11 - rest;
  };

  const digit1 = calcCheckDigit(cpf.substring(0, 9), 10);
  const digit2 = calcCheckDigit(cpf.substring(0, 10), 11);

  if (digit1 !== parseInt(cpf[9], 10) || digit2 !== parseInt(cpf[10], 10)) {
    return { cpfInvalid: true };
  }

  return null;
};


  loadHealthPlans(): void {
    this.healthPlanService.getAll().subscribe({
      next: data => this.healthPlans = data.filter(h => h.isActive),
      error: err => {
        console.error(err);
        this.error = 'Erro ao carregar convênios.';
      }
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.value;

    const request: PatientRequest = {
      ...value,
      dateOfBirth: value.dateOfBirth,
      mobilePhone: value.mobilePhone || null,
      landlinePhone: value.landlinePhone || null
    };

    this.saving = true;
    this.error = undefined;

    this.patientService.create(request).subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/patients']);
      },
      error: err => {
        this.saving = false;
        console.error(err);

        if (err.error?.error) {
          this.error = err.error.error;
        } else if (err.error?.errors) {
          const first = Object.values(err.error.errors)[0] as string[];
          this.error = first[0];
        } else {
          this.error = 'Erro ao salvar paciente.';
        }
      }
    });
  }
}
