import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Lead, LeadCreate, LeadUpdate } from '../models/lead.model';

@Injectable({
  providedIn: 'root'
})
export class LeadService {
  private readonly apiUrl = 'http://localhost:5000/api/leads';

  constructor(private http: HttpClient) {}

  getAll(search?: string, status?: number | null): Observable<Lead[]> {
    let params = new HttpParams();

    if (search) {
      params = params.set('search', search);
    }

    if (status !== null && status !== undefined) {
      params = params.set('status', status);
    }

    return this.http.get<Lead[]>(this.apiUrl, { params });
  }

  getById(id: number): Observable<Lead> {
    return this.http.get<Lead>(`${this.apiUrl}/${id}`);
  }

  create(data: LeadCreate): Observable<Lead> {
    return this.http.post<Lead>(this.apiUrl, data);
  }

  update(id: number, data: LeadUpdate): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}