import {MyConfig} from '../my-config';
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
  private apiUrl: string = MyConfig.api_address + '/api/City';

  constructor(private http: HttpClient) {}

  getCity(): Observable<CityResponse[]> {
    return this.http.get<CityResponse[]>(this.apiUrl);
  }

  getCityWithCountry(id: number){
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }
}
