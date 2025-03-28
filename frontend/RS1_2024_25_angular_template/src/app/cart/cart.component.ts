import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];
  checkoutForm!: FormGroup;
  shipping: number = 20;

  constructor(
    private fb: FormBuilder,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.cartService.loadCartItems(); // učitaj artikle iz baze

    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items.map(item => ({
        partId: item.partId,
        name: item.partName || item.name,
        quantity: item.quantity,
        price: item.price,
        image: 'http://localhost:7000/' + (item.image || 'images/placeholder.png')
      }));
    });

    this.checkoutForm = this.fb.group({
      fullName: ['', Validators.required],
      address: ['', Validators.required],
      note: ['']
    });
  }



  get totalPrice(): number {
    return this.cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);
  }

  removeItem(partId: number): void {
    this.cartService.removeItemFromCart(partId).subscribe({
      next: () => {
        this.cartService.loadCartItems();
      },
      error: (err) => {
        console.error('Error deleting item:', err);
      }
    });
  }

  submitOrder(): void {
    if (this.checkoutForm.invalid || this.cartItems.length === 0) return;

    const order = {
      fullName: this.checkoutForm.value.fullName,
      address: this.checkoutForm.value.address,
      note: this.checkoutForm.value.note,
      items: this.cartItems.map(item => ({
        partId: item.partId,
        quantity: item.quantity
      }))
    };

    console.log('Simulacija narudžbe:', order);
    alert('Ovo je simulacija slanja narudžbe.');
  }



}
