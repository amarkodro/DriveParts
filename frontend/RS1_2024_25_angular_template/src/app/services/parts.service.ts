import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PartsService {
  private apiUrl = 'http://localhost:7000/api/parts';

  constructor(private http: HttpClient) {}

  getFeaturedParts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/featured`);
  }

  getNewArrivalParts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/newArrival`);
  }

  getOnSaleParts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/onSale`);
  }
}
