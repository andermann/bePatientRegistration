import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class CustomValidators {
  static cpf(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = (control.value || '').replace(/\D/g, '');
      if (!value) {
        return null; // opcional
      }
      if (value.length !== 11) {
        return { cpf: 'CPF deve conter 11 dígitos.' };
      }
      if (/^(\d)\1{10}$/.test(value)) {
        return { cpf: 'CPF inválido.' };
      }

      const calcCheck = (base: string, factor: number): number => {
        let total = 0;
        for (let i = 0; i < base.length; i++) {
          total += parseInt(base[i], 10) * factor--;
        }
        const rest = total % 11;
        return rest < 2 ? 0 : 11 - rest;
      };

      const d1 = calcCheck(value.substring(0, 9), 10);
      const d2 = calcCheck(value.substring(0, 10), 11);

      if (d1 !== parseInt(value[9], 10) || d2 !== parseInt(value[10], 10)) {
        return { cpf: 'CPF inválido.' };
      }

      return null;
    };
  }

  static atLeastOnePhone(mobileField: string, landlineField: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const mobile = group.get(mobileField)?.value;
      const landline = group.get(landlineField)?.value;

      if (!mobile && !landline) {
        return { phoneRequired: 'Informe pelo menos um telefone (celular ou fixo).' };
      }
      return null;
    };
  }

  static notFutureDate(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) {
        return null;
      }
      const date = new Date(value);
      const today = new Date();
      if (date > today) {
        return { futureDate: 'Data não pode ser futura.' };
      }
      return null;
    };
  }
}
