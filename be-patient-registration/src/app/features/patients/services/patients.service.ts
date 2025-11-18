import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import {
  Patient,
  CreatePatientRequest,
  UpdatePatientRequest
} from '../../../core/models/patient.model';

@Injectable({
  providedIn: 'root'
})
export class PatientsService {
  private readonly baseUrl = `${environment.baseApiUrl}/Patients`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.baseUrl);
  }

  getById(id: string): Observable<Patient> {
    return this.http.get<Patient>(`${this.baseUrl}/${id}`);
  }

  create(request: CreatePatientRequest): Observable<Patient> {
    // return this.http.post<Patient>(this.baseUrl, request);
    return this.http.post<Patient>(`${this.baseUrl}`, request);
  }

  update(id: string, request: UpdatePatientRequest): Observable<Patient> {
    return this.http.put<Patient>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  

}
