import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiService } from './api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionsService {

  private http = inject(HttpClient);
  private api = inject(ApiService);

  getSubscriptions(): Observable<any[]> {
    return this.http.get<any[]>(`${this.api.apiUrl}/subscriptions`);
  }
}