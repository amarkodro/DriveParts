import { Component, OnInit, ElementRef, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth-services/auth.service';
import { CartService } from '../services/cart.service';
import { ToastrService } from 'ngx-toastr';
import { FavoritesService } from '../services/favorites.service';

interface Part {
    id?: number;
    partId?: number;
    name: string;
    price: number;
    description: string;
    partImage: string;
    manufacturerName?: string;
    categoryName?: string;
    quantity?: number;
}

@Component({
    selector: 'app-my-favorites',
    templateUrl: './my-favorites.component.html',
    styleUrls: ['./my-favorites.component.css']
})
export class MyFavoritesComponent implements OnInit {
    favoriteParts: Part[] = [];
    selectedPart: Part | null = null;

    constructor(
        private router: Router,
        private authService: AuthService,
        private cartService: CartService,
        private toastr: ToastrService,
        private favoritesService: FavoritesService,
        private renderer: Renderer2,
        private el: ElementRef
    ) { }

    ngOnInit(): void {
        this.loadFavorites();
    }

    loadFavorites() {
        this.favoritesService.getFavorites().subscribe({
            next: (parts) => {
                this.favoriteParts = parts;
                setTimeout(() => this.checkVisibility(), 300);
            },
            error: (err) => {
                console.error(err);
                this.toastr.error('Failed to load favorites');
            }
        });
    }

    removeFavorite(part: Part, event: Event) {
        event.stopPropagation();
        const id = part.partId || part.id;
        if (!id) return;

        this.favoritesService.toggleFavorite(id).subscribe({
            next: (res) => {
                if (!res.isFavorite) {
                    this.favoriteParts = this.favoriteParts.filter(p => (p.partId || p.id) !== id);
                    this.toastr.info('Removed from favorites');
                }
            },
            error: (err) => this.toastr.error('Failed to remove favorite')
        });
    }

    addToCart(part: Part, event: MouseEvent): void {
        const token = this.authService.getTokenUser();
        if (!token) {
            this.toastr.warning('Please login to add item to cart.', 'Not logged in');
            this.router.navigate(['/login']);
            return;
        }

        const quantity = part.quantity || 1;
        const id = part.partId || part.id;
        if (!id) return;

        this.cartService.addToCart(id, quantity).subscribe({
            next: (res) => {
                this.toastr.success(`${part.name} added to cart`, 'Success');
                this.cartService.loadCartItems();
                if (this.selectedPart) {
                    this.closeProductModal();
                }
            },
            error: (err) => {
                this.toastr.error('Could not add to cart', 'Error');
            }
        });
    }

    openProductModal(part: Part) {
        this.selectedPart = { ...part, quantity: 1 };
    }

    closeProductModal() {
        this.selectedPart = null;
    }

    increaseQuantity() {
        if (this.selectedPart && (this.selectedPart.quantity || 0) < 10) {
            this.selectedPart.quantity = (this.selectedPart.quantity || 0) + 1;
        }
    }

    decreaseQuantity() {
        if (this.selectedPart && (this.selectedPart.quantity || 0) > 1) {
            this.selectedPart.quantity = (this.selectedPart.quantity || 0) - 1;
        }
    }

    checkVisibility(): void {
        const cards = this.el.nativeElement.querySelectorAll('.product-card');
        cards.forEach((card: HTMLElement) => {
            this.renderer.addClass(card, 'visible');
        });
    }
}
