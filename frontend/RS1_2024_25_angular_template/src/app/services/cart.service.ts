import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Observable, Subject, BehaviorSubject} from 'rxjs';
import {tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'http://localhost:7000/api/cart';
  private cartUpdatedSoruce = new Subject<void>();
  cartUpdated$ = this.cartUpdatedSoruce.asObservable();
  private cartItemsSubject = new BehaviorSubject<any[]>([]);
  cartItems$ = this.cartItemsSubject.asObservable();


  constructor(private http: HttpClient) {}

  addToCart(partId: number, quantity: number) {
    const body = { partId, quantity };
    return this.http.post(`${this.apiUrl}/add`, body).pipe(
      tap(() => this.loadCartItems()))
  }

  getCartItems(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/getAll`);
  }
  loadCartItems() {
    this.http.get<any[]>(`${this.apiUrl}/getAll`).subscribe({
      next: (items) => this.cartItemsSubject.next(items),
      error: (err) => console.error('Error loading cart items', err)
    });
  }
  removeItemFromCart(partId: number) {
    return this.http.delete(`${this.apiUrl}/remove/${partId}`).pipe(
      tap(() => this.loadCartItems())
    );
  }

  notifyCartUpdate(){
    this.cartUpdatedSoruce.next();
  }

  clearCart(){
    return this.http.delete(`${this.apiUrl}/clear`);
  }

  removeItem(partId: number) {
    const updated = this.cartItemsSubject.value.filter(i => i.partId !== partId);
    this.cartItemsSubject.next(updated);
  }

  getItems(): any[] {
    return this.cartItemsSubject.value;
  }

  updateQuantity(partId: number, quantity: number) {
    return this.http.put(`${this.apiUrl}/update`, { partId, quantity });
  }
}
