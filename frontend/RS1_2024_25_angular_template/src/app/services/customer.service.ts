// customer.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private apiUrl = 'http://localhost:7000/api'; // Replace with your API base URL

  constructor(private http: HttpClient) { }

  getCustomers(search: string, role: string, page: number, pageSize: number): Observable<any> {
    const params = new HttpParams()
      .set('search', search)
      .set('role', role)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get(`${this.apiUrl}/users`, { params });
  }

  getCustomerOrders(customerId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/orders/by-customer/${customerId}`);
  }
}