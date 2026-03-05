import {MyConfig} from '../my-config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Part {
  partId: number;
  name: string;
  price: number;
  categoryId: number;      // Not categoryname
  manufacturerId: number;  // Not manufacturername
  partImage: string;
  description: string;
  isFeatured: boolean;
  isOnSale: boolean;
  isNewArrival: boolean;
  type: string;
}
export interface CategoryResponse {
  categoryId: number;
  name: string;
}

export interface ManufacturerResponse {
  manufacturerId: number;
  name: string;
  contact?: string;
  address?: string;
}
@Injectable({
  providedIn: 'root'
})
export class PartService {
  updatePartFormData(id: number, partData: FormData): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, partData);
  }
  private apiUrl = MyConfig.api_address + '/api/parts'; // adjust if needed
  updatePartWithFormData(id: number, formData: FormData): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/form`, formData);
  }
  constructor(private http: HttpClient) { }

  getParts(
    page: number = 1,
    pageSize: number = 10,
    name?: string,
    categoryId?: number,
    manufacturerId?: number,
    minPrice?: number,
    maxPrice?: number
  ): Observable<any> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (name) params = params.set('name', name);
    if (categoryId) params = params.set('categoryId', categoryId.toString());
    if (manufacturerId) params = params.set('manufacturerId', manufacturerId.toString());
    if (minPrice !== undefined && minPrice !== null) params = params.set('minPrice', minPrice.toString());
    if (maxPrice !== undefined && maxPrice !== null) params = params.set('maxPrice', maxPrice.toString());

    return this.http.get<any>(this.apiUrl, { params });
  }

  getPart(id: number): Observable<Part> {
    return this.http.get<Part>(`${this.apiUrl}/${id}`);
  }

  addPart(partData: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, partData);
  }
  updatePart(id: number, part: Part): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, part);
  }

  deletePart(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
  getCategories(): Observable<any[]> {
    return this.http.get<any[]>(MyConfig.api_address + '/api/categories');
  }

  getManufacturers(): Observable<any[]> {
    return this.http.get<any[]>(MyConfig.api_address + '/api/manufacturers');
  }

  getPartSuggestions(query: string): Observable<string[]> {
    const params = new HttpParams().set('query', query);
    return this.http.get<string[]>(`${this.apiUrl}/suggestions`, { params });
  }
}