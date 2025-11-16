import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Patient, PatientRequest } from '../models/patient.model';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private readonly baseUrl = `${environment.apiUrl}/api/Patients`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.baseUrl);
  }

  getById(id: string): Observable<Patient> {
    return this.http.get<Patient>(`${this.baseUrl}/${id}`);
  }

  create(request: PatientRequest): Observable<Patient> {
    return this.http.post<Patient>(this.baseUrl, request);
  }

  update(id: string, request: PatientRequest): Observable<Patient> {
    return this.http.put<Patient>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
