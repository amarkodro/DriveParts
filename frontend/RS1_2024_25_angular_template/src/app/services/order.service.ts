import {MyConfig} from '../my-config';
import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import { Order } from '@stripe/stripe-js';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private apiUrl = MyConfig.api_address + '/api/Orders';

  constructor(private http: HttpClient, ) { }

  createOrder(orderData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/add`, orderData);
  }

  getOrdersByCustomer(customerId: number): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/by-customer/${customerId}`);
  }

 // Add this method to OrderService
cancelOrder(orderId: number): Observable<any> {
  // Use the same endpoint as status updates
  return this.http.put(`${this.apiUrl}/${orderId}`, { 
    statusId: 6  // 6 is the statusId for 'Cancelled'
  });
}

}
