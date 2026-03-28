import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {OrderService} from '../services/order.service';
import {AuthService} from '../services/auth-services/auth.service';
import {CartService} from '../services/cart.service';
import {ToastrService} from 'ngx-toastr';
import {take} from 'rxjs';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-order-success',
  templateUrl: './order-success.component.html',
  styleUrl: './order-success.component.css'
})
export class OrderSuccessComponent implements OnInit {

  promoCodeId: number | null = null;
  selectedSupplierId: number | null = null;


  constructor(private router: Router,
              private orderService: OrderService,
              private authService: AuthService,
              private cartService: CartService,
              private toastr: ToastrService,
              ) { }

  ngOnInit(): void {

    this.authService.getUserProfile().subscribe({
      next: user => {
        this.authService.setUserInfo(user);
      },
      error: err => {
        console.error("Failed to refresh user info after payment:", err);
      }
    });

    const userId = this.authService.getUserId();


    const orderKey = localStorage.getItem(`orderKey-${userId}`);
    if (!orderKey) {
      console.warn("No order key found — possible duplicate request.");
      return;
    }

    const alreadyUsed = localStorage.getItem(`orderKey-used-${orderKey}`);
    if (alreadyUsed === 'true') {
      console.warn("Order already processed — skipping duplicate.");
      return;
    }


    localStorage.setItem(`orderKey-used-${orderKey}`, 'true');

    this.promoCodeId = this.cartService.getPromoCodeId(userId);
    const discount = this.cartService.getDiscount(userId) || 0;
    const savedId = localStorage.getItem(`supplierId-${userId}`);
    if (savedId) {
      this.selectedSupplierId = parseInt(savedId, 10);
    }

    const paymentIdFromStorage = localStorage.getItem(`paymentId-${userId}`);
    const paymentId = paymentIdFromStorage ? parseInt(paymentIdFromStorage, 10) : 1;

    this.cartService.cartItems$.pipe(take(1)).subscribe(cartItems => {
      const activeCartItems = cartItems.filter(item => !item.isSavedForLater);

      if (activeCartItems.length === 0) {
        console.warn("Cart is empty — skipping order creation.");
        return;
      }

      const orderItems = cartItems.map(item => ({
        partId: item.partId,
        quantity: item.quantity
      }));

      const orderData = {
        date: new Date(),
        statusId: 1,
        userId: userId,
        supplierId: this.selectedSupplierId,
        paymentId: paymentId,
        promoCodeId: this.promoCodeId && this.promoCodeId > 0 ? this.promoCodeId : null,
        items: orderItems
      };

      this.orderService.createOrder(orderData).subscribe({
        next: () => {
          Swal.fire({
            title: 'Order Completed!',
            text: 'Your order has been placed successfully. Thank you for shopping with us!',
            icon: 'success',
            confirmButtonText: 'OK',
            confirmButtonColor: '#28a745'
          });

          this.cartService.loadCartItems();
          localStorage.removeItem(`promoCodeId-${userId}`);
          localStorage.removeItem(`usedCode-${userId}`);
          localStorage.removeItem(`discount-${userId}`);
          localStorage.removeItem(`supplierId-${userId}`);
          localStorage.removeItem(`paymentId-${userId}`);
          localStorage.removeItem(`orderKey-${userId}`);

        },
        error: () => {
          this.toastr.error("Order creation failed.");

          localStorage.setItem(`orderKey-used-${orderKey}`, 'false');
        }
      });
    });
  }


}
