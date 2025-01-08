import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PartService {
  private apiUrl = 'http://localhost:7000/api/parts';

  constructor(private http: HttpClient) {}

  getAllParts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getPartById(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }
}
