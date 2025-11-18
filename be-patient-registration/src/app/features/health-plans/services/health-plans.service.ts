import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import {
  HealthPlan,
  CreateHealthPlanRequest,
  UpdateHealthPlanRequest
} from '../../../core/models/health-plan.model';

@Injectable({
  providedIn: 'root'
})
export class HealthPlansService {
  private readonly baseUrl = `${environment.baseApiUrl}/HealthPlans`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<HealthPlan[]> {
    return this.http.get<HealthPlan[]>(this.baseUrl);
  }

  getById(id: string): Observable<HealthPlan> {
    return this.http.get<HealthPlan>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateHealthPlanRequest): Observable<HealthPlan> {
    return this.http.post<HealthPlan>(this.baseUrl, request);
  }

  update(id: string, request: UpdateHealthPlanRequest): Observable<HealthPlan> {
    return this.http.put<HealthPlan>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
