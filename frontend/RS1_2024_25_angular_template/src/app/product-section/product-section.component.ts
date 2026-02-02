import {Component, ElementRef, OnInit} from '@angular/core';
import { PartsService } from '../services/parts.service';
import { Router } from '@angular/router';
import {CartService} from '../services/cart.service';
import {AuthService} from '../services/auth-services/auth.service';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-product-section',
  templateUrl: './product-section.component.html',
  styleUrls: ['./product-section.component.css'],
})
export class ProductSectionComponent implements OnInit {
  featuredParts: any[] = [];
  newArrivalParts: any[] = [];
  onSaleParts: any[] = [];
  selectedProduct: any = null;
  wishlistPopupVisible: boolean = false;
  wishlistPopupPosition = { top: '0px', left: '0px' };


  cartItems: any[] = [];
  selectedQuantity: any;


  constructor(private partsService: PartsService,
              private router: Router,
              private elRef: ElementRef,
              private cartService : CartService,
              private authService: AuthService,
              private toastr: ToastrService,
              ) {}

  ngOnInit(): void {
    this.loadFeaturedParts();
    this.loadNewArrivalParts();
    this.loadOnSaleParts();
    this.loadCartFromStorage();
  }

  loadFeaturedParts(): void {
    this.partsService.getFeaturedParts().subscribe({
      next: (data) => {
        this.featuredParts = data;
      },
      error: (err) => {
        console.error('Error fetching featured parts:', err);
      },
    });
  }

  loadNewArrivalParts(): void {
    this.partsService.getNewArrivalParts().subscribe({
      next: (data) => {
        this.newArrivalParts = data;
      },
      error: (err) => {
        console.error('Error fetching new arrivals:', err);
      },
    });
  }

  loadOnSaleParts(): void {
    this.partsService.getOnSaleParts().subscribe({
      next: (data) => {
        this.onSaleParts = data;
      },
      error: (err) => {
        console.error('Error fetching on-sale parts:', err);
      },
    });
  }

  openProductModal(part: any): void {
    console.log("Select product: ", part)
    this.selectedProduct = part;
    this.selectedProduct = { ...part, quantity: this.selectedProduct?.quantity || 1 };
  }

  closeProductModal(): void {
    this.selectedProduct = null;
  }
  addToCart(part: any, event: MouseEvent): void {
    const token = this.authService.getTokenUser();

    if (!token) {
      this.toastr.warning('Please login to add item to cart.', 'Not logged in');
      this.router.navigate(['/login']);
      return;
    }

    const quantity = part.quantity || this.selectedProduct?.quantity || 1;

    this.cartService.addToCart(part.partId, quantity).subscribe({
      next: () => {
        this.toastr.success(`${part.name} added to cart`, 'Success');
        this.cartService.notifyCartUpdate();

        if (this.selectedProduct) {
          this.flyToCartFromModal('http://localhost:7000' + part.partImage);
          this.closeProductModal();
        } else if (event) {
          this.flyToCart(event);
        }
      },
      error: (err) => {
        console.error('Add to cart failed:', err);
        this.toastr.error('Could not add to cart', 'Error');
      }
    });
  }



  removeFromCart(part: any): void {
    this.cartItems = this.cartItems.filter(item => item.id !== part.id);
    this.saveCartToStorage();
    console.log(`Removed from cart: ${part.name}`);
  }

  saveCartToStorage(): void {
    localStorage.setItem('cartItems', JSON.stringify(this.cartItems));
  }

  loadCartFromStorage(): void {
    const storedCart = localStorage.getItem('cartItems');
    if (storedCart) {
      this.cartItems = JSON.parse(storedCart);
    }
  }

  toggleWishlistPopup(event: MouseEvent): void {
    event.stopPropagation();
    this.wishlistPopupVisible = !this.wishlistPopupVisible;

    if (this.wishlistPopupVisible) {
      // Gets the position of the clicked element
      const target = event.target as HTMLElement;
      const rect = target.getBoundingClientRect();
      this.wishlistPopupPosition = {
        top: `${rect.bottom + window.scrollY}px`,
        left: `${rect.left + window.scrollX}px`,
      };
    }
  }

  increaseQuantity(): void {
    if (this.selectedProduct && this.selectedProduct.quantity < 10) {
      this.selectedProduct.quantity++;
      console.log("Quantity is: ", this.selectedProduct.quantity);
    }
  }

  decreaseQuantity(): void {
    if (this.selectedProduct && this.selectedProduct.quantity > 1) {
      this.selectedProduct.quantity--;
      console.log("Quantity is: ", this.selectedProduct.quantity);
    }
  }

  ngAfterViewInit(): void {
    const sections: NodeListOf<HTMLElement> = this.elRef.nativeElement.querySelectorAll('.section');

    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          entry.target.classList.add('visible');
        }
      });
    }, { threshold: 0.2 });

    sections.forEach((section) => observer.observe(section as HTMLElement));
  }

  viewAllParts(category: string) {
    this.router.navigate(['view-parts'],{queryParams: {category}});
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
    this.router.navigate(['part-detail', partId]);
  }
}
