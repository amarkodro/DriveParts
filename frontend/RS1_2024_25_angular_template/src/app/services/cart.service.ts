import {MyConfig} from '../my-config';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Observable, Subject, BehaviorSubject} from 'rxjs';
import {tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = MyConfig.api_address + '/api/cart';
  private cartUpdatedSoruce = new Subject<void>();
  cartUpdated$ = this.cartUpdatedSoruce.asObservable();
  private cartItemsSubject = new BehaviorSubject<any[]>([]);
  cartItems$ = this.cartItemsSubject.asObservable();
  private discount: number = 0;
  private cartItemId: any;
  private currentUserId: number = 0;


  constructor(private http: HttpClient, ) {
  }

  initForUser(userId: number) {
    this.currentUserId = userId;
    const savedItems = localStorage.getItem(`cartItems-${userId}`);
    if (savedItems) {
      this.cartItemsSubject.next(JSON.parse(savedItems));
    }
  }

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
      next: (items) => {
        this.cartItemsSubject.next(items);
        localStorage.setItem(`cartItems-${this.currentUserId}`, JSON.stringify(items));
      },
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

  clearCart() {
    localStorage.removeItem(`cartItems-${this.currentUserId}`);
    localStorage.removeItem(`discount-${this.currentUserId}`);
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

  setDiscount(value: number, userId: number) {
    localStorage.setItem(`discount-${userId.toString()}`, value.toString());
  }

  getDiscount(userId: number): number {
    const saved = localStorage.getItem(`discount-${userId.toString()}`);
    return saved ? parseFloat(saved) : 0;
  }

  setUsedCode(code: string, userId: number) {
    localStorage.setItem(`usedCode-${userId.toString()}`, code);
  }

  getUsedCode(userId: number): string {
    return localStorage.getItem(`usedCode-${userId.toString()}`) || '';
  }

  clearCouponData(userId: number): void {
    localStorage.removeItem(`discount-${userId.toString()}`);
    localStorage.removeItem(`usedCode-${userId.toString()}`);
  }

  setPromoCodeId(id: number, userId: number): void {
    localStorage.setItem(`promoCodeId-${userId}`, id.toString());
  }

  getPromoCodeId(userId: number): number | null {
    const id = localStorage.getItem(`promoCodeId-${userId}`);
    return id ? parseInt(id, 10) : null;
  }


  saveForLater(cartItemId: number) {
    return this.http.put(
      `${MyConfig.api_address}/api/cart/${cartItemId}/save-to-later`,
      {}
    );
  }

  moveToCart(cartItemId: number) {
    return this.http.put(
      `${MyConfig.api_address}/api/cart/${cartItemId}/move-to-cart`,
      {}
    );
  }
}
