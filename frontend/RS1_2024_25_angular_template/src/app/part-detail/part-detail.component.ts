import { MyConfig } from '../my-config';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PartService } from '../services/navbar-search.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { CartService } from '../services/cart.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth-services/auth.service';


@Component({
  selector: 'app-part-detail',
  templateUrl: './part-detail.component.html',
  styleUrls: ['./part-detail.component.css']
})
export class PartDetailComponent implements OnInit {
  apiUrl = MyConfig.api_address;
  partId: string | null = null;
  part: any = null;

  constructor(private route: ActivatedRoute, private partService: PartService, private cartService: CartService, private toastr: ToastrService, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.partId = paramMap.get('id');
      if (this.partId) {
        this.fetchPartDetails(this.partId);
      }
    });
  }

  fetchPartDetails(id: string): void {
    this.partService.getPartById(id).subscribe(
      (data) => {
        this.part = data;
      },
      (error) => {
        console.error('Error fetching part details:', error);
      }
    );
  }

  quantity = 1;

  increaseQuantity(): void {
    if (this.quantity < 10) this.quantity++;
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) this.quantity--;
  }

  addToCart(event?: MouseEvent): void {
    if (!this.authService.isLoggedIn()) {
      this.toastr.info('Please log in to add items to cart', 'Login required');

      this.router.navigate(['/login'], {
        queryParams: { returnUrl: this.router.url }
      });
      return;
    }

    if (!this.part) return;

    const quantity = this.quantity;

    this.cartService.addToCart(this.part.partId, quantity).subscribe({
      next: () => {
        this.toastr.success(`${this.part.name} has been added to cart`, 'Success');
        this.cartService.notifyCartUpdate();

        // animacija: sa slike na detail stranici do cart ikone
        if (event) this.flyToCartFromDetail();
      },
      error: (err) => {
        console.error('Adding to cart failed:', err);
        this.toastr.error('Unable to add to cart', 'Error');
      }
    });
  }


  flyToCartFromDetail() {
    const image = document.querySelector('.part-image') as HTMLImageElement;
    const cart = document.getElementById('cart-icon');

    if (!image || !cart) return;

    const imgRect = image.getBoundingClientRect();
    const cartRect = cart.getBoundingClientRect();

    const imgClone = image.cloneNode(true) as HTMLImageElement;
    imgClone.style.position = 'fixed';
    imgClone.style.left = imgRect.left + 'px';
    imgClone.style.top = imgRect.top + 'px';
    imgClone.style.width = imgRect.width + 'px';
    imgClone.style.height = imgRect.height + 'px';
    imgClone.style.zIndex = '1000';
    imgClone.style.transition = 'all 0.8s ease-in-out';
    imgClone.style.borderRadius = '50%';
    imgClone.style.pointerEvents = 'none';
    document.body.appendChild(imgClone);

    requestAnimationFrame(() => {
      imgClone.style.left = cartRect.left + 'px';
      imgClone.style.top = cartRect.top + 'px';
      imgClone.style.width = '0px';
      imgClone.style.height = '0px';
      imgClone.style.opacity = '0.5';
    });

    setTimeout(() => imgClone.remove(), 900);
  }

}