import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DropdownItem {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root',
})
export class DropdownService {
  private apiUrl = 'http://localhost:7000/api/filter';

  constructor(private http: HttpClient) {}

  getCars(): Observable<DropdownItem[]> {
    return this.http.get<DropdownItem[]>(`${this.apiUrl}/cars`);
  }

  getCategories(): Observable<DropdownItem[]> {
    return this.http.get<DropdownItem[]>(`${this.apiUrl}/categories`);
  }

  getParts(): Observable<DropdownItem[]> {
    return this.http.get<DropdownItem[]>(`${this.apiUrl}/parts`);
  }

  getModels(carId: number): Observable<DropdownItem[]> {
    console.log('Calling API with carId:', carId);
    const params = new HttpParams().set('carId', carId.toString());
    return this.http.get<DropdownItem[]>(`${this.apiUrl}/models`, { params });
  }

  filterParts(params: any): Observable<any[]> {
    console.log('Params sent to API:', params);
    return this.http.get<any[]>(`${this.apiUrl}/filter`, { params });
  }

  getVehicleTypes(): Observable<DropdownItem[]> {
    return this.http.get<DropdownItem[]>(`${this.apiUrl}/types`);
  }
}
