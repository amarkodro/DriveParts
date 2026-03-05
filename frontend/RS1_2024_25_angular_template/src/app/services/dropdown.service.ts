import {MyConfig} from '../my-config';
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
  private apiUrl = MyConfig.api_address + '/api/filter';

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
    const params = new HttpParams().set('carId', carId.toString());
    return this.http.get<DropdownItem[]>(`${this.apiUrl}/models`, { params });
  }

  filterParts(params: any): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/filter`, { params });
  }

  getVehicleTypes(): Observable<DropdownItem[]> {
    return this.http.get<DropdownItem[]>(`${this.apiUrl}/types`);
  }
}