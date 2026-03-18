import { MyConfig } from '../my-config';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PartsService } from '../services/parts.service';
import { CartService } from '../services/cart.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth-services/auth.service';

@Component({
  selector: 'app-view-parts',
  templateUrl: './view-parts.component.html',
  styleUrl: './view-parts.component.css'
})
export class ViewPartsComponent implements OnInit {
  apiUrl = MyConfig.api_address;
  parts: any[] = [];
  category: string = '';
  selectedProduct: any;

  constructor(private route: ActivatedRoute,
    private partsService: PartsService,
    private cartService: CartService,
    private toastr: ToastrService,
    private router: Router
  ) { }
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.category = params['category'] || 'all';
      this.category = this.category.replace('-', ' ');
      this.loadParts();
    });

  }

  private loadParts() {
    if (this.category === 'featured') {
      this.partsService.getFeaturedParts().subscribe(data => this.parts = data);
    } else if (this.category === 'on sale') {
      this.partsService.getOnSaleParts().subscribe(data => this.parts = data);
    } else if (this.category === 'new arrivals') {
      this.partsService.getNewArrivalParts().subscribe(data => this.parts = data);
    }
  }

  openProductModal(part: any): void {
    this.selectedProduct = part;
    this.selectedProduct = { ...part, quantity: this.selectedProduct?.quantity || 1 };
  }

  closeProductModal(): void {
    this.selectedProduct = null;
  }

  increaseQuantity(): void {
    if (this.selectedProduct && this.selectedProduct.quantity < 10) {
      this.selectedProduct.quantity++;
    }
  }

  decreaseQuantity(): void {
    if (this.selectedProduct && this.selectedProduct.quantity > 1) {
      this.selectedProduct.quantity--;
    }
  }

  addToCart(part: any, event: MouseEvent): void {
    const quantity = part.quantity || this.selectedProduct?.quantity || 1;
    this.cartService.addToCart(part.partId, quantity).subscribe({
      next: () => {
        this.toastr.success(`${part.name} has been added to cart`, 'Success');
        this.cartService.notifyCartUpdate(); // Obavijesti navbar da se korpa ažurira

        if (this.selectedProduct) {
          this.flyToCartFromModal(MyConfig.api_address + part.partImage);
          this.closeProductModal();
        } else if (event) {
          this.flyToCart(event);
        }
      },
      error: (err) => {
        console.error('Adding to cart failed:', err);
        this.toastr.error('Unable to add to cart', 'Error');
      }
    });
  }


  flyToCart(event: MouseEvent) {
    const image = (event.target as HTMLElement)
      .closest('.product-card')
      ?.querySelector('.product-image') as HTMLImageElement;

    const cart = document.getElementById('cart-icon');

    if (!image || !cart) return;

    const imgClone = image.cloneNode(true) as HTMLImageElement;
    const imgRect = image.getBoundingClientRect();
    const cartRect = cart.getBoundingClientRect();

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

  flyToCartFromModal(imageUrl: string) {
    const cart = document.getElementById('cart-icon');
    const modalImage = document.querySelector('.modal-image') as HTMLElement;

    if (!modalImage || !cart) return;

    const imgClone = document.createElement('img');
    imgClone.src = imageUrl;

    const imgRect = modalImage.getBoundingClientRect();
    const cartRect = cart.getBoundingClientRect();

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

  goToPartDetails(partId: any) {
    this.router.navigate(['/part-detail', partId]);
  }
}