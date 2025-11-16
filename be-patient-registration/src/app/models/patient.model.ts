export interface Patient {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  dateOfBirth: string;
  gender: number;
  cpf?: string | null;
  rg: string;
  ufRg: number;
  email: string;
  mobilePhone: string;
  landlinePhone?: string | null;
  healthPlanId: string;
  healthPlanName: string;
  healthPlanCardNumber: string;
  healthPlanCardExpirationMonth: number;
  healthPlanCardExpirationYear: number;
  isActive: boolean;
  createdAt: string;
}

export interface PatientRequest {
  firstName: string;
  lastName: string;
  dateOfBirth: string; // yyyy-MM-dd
  gender: number;
  cpf?: string | null;
  rg: string;
  ufRg: number;
  email: string;
  mobilePhone: string | null;
  landlinePhone: string | null;
  healthPlanId: string;
  healthPlanCardNumber: string;
  healthPlanCardExpirationMonth: number;
  healthPlanCardExpirationYear: number;
}
