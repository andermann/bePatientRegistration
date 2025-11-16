import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { HealthPlan } from '../models/health-plan.model';

@Injectable({ providedIn: 'root' })
export class HealthPlanService {
  private readonly baseUrl = `${environment.apiUrl}/api/HealthPlans`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<HealthPlan[]> {
    return this.http.get<HealthPlan[]>(this.baseUrl);
  }
}
