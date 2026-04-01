import {MyConfig} from '../my-config';
import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { CartService } from '../services/cart.service';
import {ToastrService} from 'ngx-toastr';
import {PromoCodeService} from '../services/promo-code.service';
import {Router} from '@angular/router';
import {AuthService} from '../services/auth-services/auth.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];
  savedItems: any[] = [];
  discount: number = 0;
  invalidCoupon: boolean = false;
  checkoutForm!: FormGroup;
  usedCode: string = '';
  isApplyingCoupon: boolean = false;
  isCheckingOut: boolean = false;


  constructor(
    private fb: FormBuilder,
    private cartService: CartService,
    private toastr: ToastrService,
    private promoService: PromoCodeService,
    private router: Router,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.checkoutForm = this.fb.group({
      couponCode: ['', Validators.required]
    });

    const userId = this.authService.getUserId();
    this.discount = this.cartService.getDiscount(userId);
    this.usedCode = this.cartService.getUsedCode(userId);

    this.cartService.loadCartItems();

    this.cartService.cartItems$.subscribe(items => {
      const mapped = items.map(item => ({
        id: item.id ?? item.Id,
        partId: item.partId,
        name: item.partName || item.name,
        quantity: item.quantity,
        price: item.price,
        image: MyConfig.api_address + '/' + (item.image || 'images/placeholder.png'),
        isSavedForLater: item.isSavedForLater
      }));

      this.cartItems = mapped.filter(x => !x.isSavedForLater);
      this.savedItems = mapped.filter(x => x.isSavedForLater);
    });
  }

  get totalPrice(): number {
    return this.cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);
  }

  get tax(): number {
    return (this.totalPrice - this.discount) * 0.17;
  }

  get finalTotal(): number {
    return (this.totalPrice - this.discount) * 1.17; // ✅ Sa porezom
  }

  applyCoupon(): void {
    const code = this.checkoutForm.get('couponCode')?.value?.trim().toUpperCase();
    const userId = this.authService.getUserId();

    if (!code) {
      this.toastr.warning('Please enter a promo code');
      return;
    }

    this.isApplyingCoupon = true;

    this.promoService.checkCode(code).subscribe({
      next: res => {
        const discount = res.discount;
        const promoId = res.id;

        this.discount = this.totalPrice * (discount / 100);
        this.cartService.setDiscount(this.discount, userId);
        this.cartService.setUsedCode(code, userId);
        this.cartService.setPromoCodeId(promoId, userId);
        this.invalidCoupon = false;
        this.usedCode = code;
        setTimeout(() => {
          this.toastr.success(`Coupon applied: ${discount}%`);
          this.isApplyingCoupon = false;
        }, 2000);
      },
      error: err => {
        this.discount = 0;
        this.cartService.setDiscount(0, userId);
        this.cartService.setUsedCode('', userId);
        this.cartService.setPromoCodeId(0, userId);
        this.invalidCoupon = true;


        setTimeout(() => {
          this.toastr.error('Invalid promo code');
          this.isApplyingCoupon = false;
        }, 2000);
      }
    });
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

  }

  onQuantityChanged(item: any): void {
    const userId = this.authService.getUserId();

    if (item.quantity > 10) {
      item.quantity = 10;
      this.toastr.warning('Maximum quantity is 10');
      return;
    }

    if (item.quantity < 1) {
      item.quantity = 1;
      this.toastr.warning('Minimum quantity is 1');
      return;
    }

    this.cartService.updateQuantity(item.partId, item.quantity).subscribe({
      next: () => {
        this.cartService.loadCartItems();
        this.toastr.success('Quantity updated', 'Success');


        if (this.usedCode) {
          this.promoService.checkCode(this.usedCode).subscribe({
            next: res => {
              const discount = res.discount;
              this.discount = this.totalPrice * (discount / 100);
              this.cartService.setDiscount(this.discount, userId);
            },
            error: err => {
              this.discount = 0;
              this.cartService.setDiscount(0, userId);
              this.invalidCoupon = true;
              this.toastr.error('Promo code is no longer valid');
            }
          });
        }
      },
      error: err => {
        console.error('Error updating quantity', err);
        this.toastr.error('Error updating quantity', err);
      }
    });
  }

  goBack() {
    this.router.navigate(['/']);
  }

  removeCoupon() {
    const userId = this.authService.getUserId();
    this.discount = 0;
    this.usedCode = '';
    this.invalidCoupon = false;
    this.cartService.setDiscount(0, userId);
    this.cartService.setPromoCodeId(0, userId);
    this.cartService.setUsedCode('', userId);
  }

  goToCheckout(): void {
    this.isCheckingOut = true;

    setTimeout(() => {
      this.router.navigate(['/checkout']);
    }, 2000);
  }

  onSaveForLater(item: any): void {
    this.cartService.saveForLater(item.id).subscribe({
      next: () => {
        this.cartService.loadCartItems();
        this.toastr.info('Item saved for later');
      },
      error: (err) => {
        console.error(err);
        this.toastr.error('Failed to save for later');
      }
    });
  }

  onMoveToCart(item: any): void {
    this.cartService.moveToCart(item.id).subscribe({
      next: () => {
        this.cartService.loadCartItems();
        this.toastr.success('Item moved to cart');
      },
      error: (err) => {
        console.error(err);
        this.toastr.error('Failed to move to cart');
      }
    });
  }





}
