import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DropdownService {
  private apiUrl = 'http://localhost:7000/api/filter';

  constructor(private http: HttpClient) {}

  getCars(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/cars`);
  }

  getCategories(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/categories`);
  }

  getParts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/parts`);
  }

  getBrands(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/manufacturers`);
  }

  filterParts(params: any): Observable<any[]> {
    console.log('Params sent to API:', params); //
    return this.http.get<any[]>('http://localhost:7000/api/filter/filter', { params });
  }

}
