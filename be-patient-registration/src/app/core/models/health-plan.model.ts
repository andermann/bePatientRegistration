export interface HealthPlan {
  id: string;
  name: string;
  isActive: boolean;
}

export interface CreateHealthPlanRequest {
  name: string;
}

export interface UpdateHealthPlanRequest {
  name: string;
  isActive: boolean;
}
