import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CityResponse {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CitiesService {
  private apiUrl: string = 'http://localhost:7000/api/City';

  constructor(private http: HttpClient) {}  // ✅ Sada HttpClient radi

  getCity(): Observable<CityResponse[]> {
    return this.http.get<CityResponse[]>(this.apiUrl);
  }
}
