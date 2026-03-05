import {MyConfig} from '../my-config';
// orders.service.ts
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
  statusId: number;
}

export interface Status {
  id: number;
  name: string; // Must match your backend's status names
}

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private apiUrl = MyConfig.api_address + '/api/orders';
statusOptions = [
   { id: 1, name: 'Pending' },    // Must match seeded names
    { id: 2, name: 'Approved' },  // Exactly as in your database
    { id: 3, name: 'Rejected' },
    { id: 4, name: 'In Progress' },
    { id: 6, name: 'Cancelled' },
    { id: 7, name: 'On Hold' },
    { id: 8, name: 'Failed' },
    { id: 9, name: 'Draft' },
    { id: 10, name: 'Submitted' },
    { id: 5, name: 'Completed' },

  ];
  constructor(private http: HttpClient) {}

  // Fetch orders
  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl);
  }

  // Fetch statuses from backend
  getStatuses(): Observable<Status[]> {
    return this.http.get<Status[]>(MyConfig.api_address + '/api/statuses'); // Update endpoint to match your API
  }

  // Update order status
updateOrderStatus(orderId: number, statusId: number): Observable<any> {
  return this.http.put(`${this.apiUrl}/${orderId}`, { 
    statusId: statusId // Match backend's expected field name
  });
}
  deleteOrder(orderId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${orderId}`);
  }
  downloadReceipt(orderId: number): Observable<Blob> {
    return this.http.get(`${MyConfig.api_address}/api/Orders/GenerateReceipt/${orderId}`, {
      responseType: 'blob' // Ensure response is treated as a Blob (binary data)
    });
  }
}