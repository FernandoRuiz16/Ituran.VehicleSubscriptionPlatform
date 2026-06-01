import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiService } from './api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AnalyticsService {
  private http = inject(HttpClient);
  private api = inject(ApiService);

  getSummary(): Observable<any> {
    return this.http.get<any>(`${this.api.apiUrl}/analytics/summary`);
  }
}