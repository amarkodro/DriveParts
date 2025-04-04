import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { CartService } from '../services/cart.service';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];
  discount: number = 0;
  invalidCoupon: boolean = false;
  checkoutForm!: FormGroup;



  constructor(
    private fb: FormBuilder,
    private cartService: CartService,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.checkoutForm = this.fb.group({
      couponCode: ['', Validators.required]
    });

    this.cartService.loadCartItems();

    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items.map(item => ({
        partId: item.partId,
        name: item.partName || item.name,
        quantity: item.quantity,
        price: item.price,
        image: 'http://localhost:7000/' + (item.image || 'images/placeholder.png')
      }));
    });
  }

  get totalPrice(): number {
    return this.cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);
  }

  get finalTotal(): number {
    return this.totalPrice - this.discount;
  }

  applyCoupon(): void {
    const code = this.checkoutForm.get('couponCode')?.value?.trim().toUpperCase();
    if (code === 'DRIVEPARTS10' && this.totalPrice > 500) {
      this.discount = this.totalPrice * 0.15;
      this.invalidCoupon = false;
      this.toastr.success('Coupon successfully redeemed');
    } else {
      this.discount = 0;
      this.invalidCoupon = !!code;
      this.toastr.info('Total price must be over 500 KM');
    }
  }

  removeItem(partId: number): void {
    this.cartService.removeItemFromCart(partId).subscribe({
      next: () => this.cartService.loadCartItems(),
      error: err => console.error('Error deleting item:', err)
    });
  }

  submitOrder(): void {
    if (this.cartItems.length === 0) return;

    const order = {
      couponCode: this.checkoutForm.value.couponCode,
      items: this.cartItems.map(item => ({
        partId: item.partId,
        quantity: item.quantity
      }))
    };

    console.log(order);
  }

  onQuantityChanged(item:any): void {

    if(item.quantity > 10) {
      item.quantity = 10;
      this.toastr.warning('Maximum quantity is 10');
      return;
    }

    if(item.quantity < 1 ){
      item.quantity = 1;
      this.toastr.warning('Minimum quantity is 1');
      return;
    }

    this.cartService.updateQuantity(item.partId, item.quantity).subscribe({
      next: () => {
        this.cartService.loadCartItems();
          this.toastr.success('Quantity updated' , 'Success')
      },
      error: err => {console.error('Error updating quantity',err), this.toastr.error('Error updating quantity',err)}
    });
  }

}
