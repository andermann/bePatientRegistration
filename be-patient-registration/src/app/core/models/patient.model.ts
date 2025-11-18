export enum Gender {
  Male = 0,
  Female = 1,
  Other = 2
}

export enum Uf {
  AC, AL, AP, AM, BA, CE, DF, ES, GO, MA,
  MT, MS, MG, PA, PB, PR, PE, PI, RJ, RN,
  RS, RO, RR, SC, SP, SE, TO
}

export interface Patient {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  dateOfBirth: string; // ISO string
  gender: Gender;
  cpf?: string | null;
  rg: string;
  ufRg: Uf;
  email: string;
  mobilePhone: string;
  landlinePhone?: string | null;
  healthPlanId: string;
  healthPlanCardNumber: string;
  healthPlanCardExpirationMonth: number;
  healthPlanCardExpirationYear: number;
  isActive: boolean;
}

export interface CreatePatientRequest {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  gender: Gender;
  cpf?: string | null;
  rg: string;
  ufRg: Uf;
  email: string;
  mobilePhone: string;
  landlinePhone?: string | null;
  healthPlanId: string;
  healthPlanCardNumber: string;
  healthPlanCardExpirationMonth: number;
  healthPlanCardExpirationYear: number;
  isActive: boolean;
}

export interface UpdatePatientRequest extends CreatePatientRequest {
  isActive: boolean;
}
