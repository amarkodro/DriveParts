import {Component, ElementRef, OnInit} from '@angular/core';
import { PartsService } from '../services/parts.service';
import { Router } from '@angular/router';

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


  constructor(private partsService: PartsService, private router: Router, private elRef: ElementRef) {}

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

  addToCart(part: any): void {
    const itemExists = this.cartItems.find(item => item.id === part.id);
    if (!itemExists) {
      this.cartItems.push({ ...part, quantity: this.selectedProduct.quantity });
      this.saveCartToStorage();
      console.log(`Added to cart: ${part.name}, Quantity: ${this.selectedProduct.quantity}`);
    } else {
      itemExists.quantity += this.selectedProduct.quantity; // Ako već postoji, samo povećaj količinu
      this.saveCartToStorage();
      console.log(`Updated cart: ${part.name}, Quantity: ${itemExists.quantity}`);
    }
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
}
