import { MyConfig } from '../my-config';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PartService {
  private apiUrl = MyConfig.api_address + '/api/parts';

  constructor(private http: HttpClient) { }

  getAllParts(): Observable<any> {
    return this.http.get<any>(this.apiUrl + '?pageSize=1000');
  }

  getPartById(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }
}

