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

    this.promoCodeId = this.cartService.getPromoCodeId(userId);
    const discount = this.cartService.getDiscount(userId) || 0;
    const savedId = localStorage.getItem(`supplierId-${this.authService.getUserId()}`);
    if (savedId) {
      this.selectedSupplierId = parseInt(savedId, 10);
    }

    const paymentIdFromStorage = localStorage.getItem(`paymentId-${userId}`);
    const paymentId = paymentIdFromStorage ? parseInt(paymentIdFromStorage, 10) : 1;



    this.cartService.cartItems$.pipe(take(1)).subscribe(cartItems => {
      if (cartItems.length === 0) {
        return;
      }

      const orderItems = cartItems.map(item => ({
        partId: item.partId,
        quantity: item.quantity,
        price: item.price
      }));

      const subtotal = cartItems.reduce(
        (sum, item) => sum + item.price * item.quantity, 0
      );

      const totalAmount = subtotal - discount;

      const orderData = {
        date: new Date(),
        statusId: 1,
        userId: userId,
        supplierId: this.selectedSupplierId,
        paymentId: paymentId,
        promoCodeId: this.promoCodeId,
        totalAmount: totalAmount,
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

          this.cartService.clearCart();
          this.cartService.loadCartItems();
          localStorage.removeItem(`promoCodeId-${userId}`);
          localStorage.removeItem(`usedCode-${userId}`);
          localStorage.removeItem(`discount-${userId}`);
          localStorage.removeItem(`supplierId-${userId}`);
          localStorage.removeItem(`paymentId-${userId}`);
        },
        error: (error) => {
          this.toastr.error("Order creation failed.");
        }
      });
    });


  }


}