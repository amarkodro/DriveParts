import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SuppliersService {

  private apiUrl = 'http://localhost:7000/api/Suppliers';

  constructor(private http: HttpClient) { }

  getAllSuppliers(){
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }
}
