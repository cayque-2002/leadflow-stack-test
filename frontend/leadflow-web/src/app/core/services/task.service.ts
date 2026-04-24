import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TaskCreate, TaskItem, TaskUpdate } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly apiUrl = 'http://localhost:5000/api/leads';

  constructor(private http: HttpClient) {}

  getByLead(leadId: number): Observable<TaskItem[]> {
    return this.http.get<TaskItem[]>(`${this.apiUrl}/${leadId}/tasks`);
  }

  getById(leadId: number, taskId: number): Observable<TaskItem> {
    return this.http.get<TaskItem>(`${this.apiUrl}/${leadId}/tasks/${taskId}`);
  }

  create(leadId: number, data: TaskCreate): Observable<TaskItem> {
    return this.http.post<TaskItem>(`${this.apiUrl}/${leadId}/tasks`, data);
  }

  update(leadId: number, taskId: number, data: TaskUpdate): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${leadId}/tasks/${taskId}`, data);
  }

  delete(leadId: number, taskId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${leadId}/tasks/${taskId}`);
  }
}