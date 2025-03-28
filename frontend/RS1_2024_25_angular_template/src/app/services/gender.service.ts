import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GenderService {

  private apiUrl = 'http://localhost:7000/api';

  constructor(private http: HttpClient) { }

  getGenders(){
    return this.http.get<any[]>(`${this.apiUrl}/Gender`);
  }
}
