import {MyConfig} from '../my-config';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Order {
  orderId: number;
  date: Date;
  statusName: string;
  username: string;
  supplierName: string;
  paymentMethod: string;
  promoCode?: string;
  discount?: number;
  totalAmount?: number;
}

@Injectable({
  providedIn: 'root'
})
export class MyOrdersService {
  private apiUrl = MyConfig.api_address + '/api/Orders';

  constructor(private http: HttpClient) { }

  getOrdersByCustomer(customerId: number): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/by-customer/${customerId}`);
  }

  downloadReceipt(orderId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/GenerateReceipt/${orderId}`, {
      responseType: 'blob'
    });
  }
  // Add this method to OrderService
cancelOrder(orderId: number): Observable<any> {
  // Use the same endpoint as status updates
  return this.http.put(`${this.apiUrl}/${orderId}`, { 
    statusId: 6  // 6 is the statusId for 'Cancelled'
  });
}
}