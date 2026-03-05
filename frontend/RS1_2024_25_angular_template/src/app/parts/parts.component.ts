import {MyConfig} from '../my-config';
import { Component, OnInit, AfterViewInit, ElementRef, Renderer2, HostListener } from '@angular/core';
import { DropdownService } from '../services/dropdown.service';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth-services/auth.service';
import { CartService } from '../services/cart.service';
import { ToastrService } from 'ngx-toastr';
import { FavoritesService } from '../services/favorites.service';

interface DropdownItem {
  id: number;
  name: string;
}

interface Part {

  partId: number;
  id: number;
  name: string;
  price: number;
  description: string;
  partImage: string;
  categoryId?: number;
  manufacturerId?: number;
  categoryName?: string;
  manufacturerName?: string;
  quantity?: number;
}

@Component({
  selector: 'app-parts',
  templateUrl: './parts.component.html',
  styleUrls: ['./parts.component.css'],
})
export class PartsComponent implements OnInit, AfterViewInit {
  cars: DropdownItem[] = [];
  categories: DropdownItem[] = [];
  parts: DropdownItem[] = [];
  models: DropdownItem[] = [];
  types: DropdownItem[] = [];
  filteredParts: Part[] = [];

  selectedCarId: number | null = null;
  selectedCategoryId: number | null = null;
  selectedPartId: number | null = null;
  selectedModelId: number | null = null;
  selectedTypeId: number | null = null;
  dropdownState: { [key: string]: boolean } = {
    car: false,
    model: false,
    category: false,
    part: false,
    type: false
  };
  showVehicleTypeDropdown: boolean = false;
  frontCategoryId: number = 11;
  selectedPart: any = null;
  favoriteIds: number[] = [];

  constructor(
    private dropdownService: DropdownService,
    private router: Router,
    private renderer: Renderer2,
    private el: ElementRef,
    private authService: AuthService,
    private cartService: CartService,
    private toastr: ToastrService,
    private favoritesService: FavoritesService
  ) { }

  ngOnInit(): void {
    this.loadDropdowns();
    this.loadFavorites();
  }

  loadFavorites() {
    if (this.authService.isLoggedIn()) {
      this.favoritesService.getFavoriteIds().subscribe({
        next: (ids) => this.favoriteIds = ids,
        error: (err) => console.error('Error loading favorites:', err)
      });
    }
  }

  isFavorite(partId: number | undefined): boolean {
    if (!partId) return false;
    return this.favoriteIds.includes(partId);
  }

  toggleFavorite(part: any, event: Event) {
    event.stopPropagation();
    if (!this.authService.isLoggedIn()) {
      this.toastr.warning('Please login to add to favorites');
      return;
    }

    const partIdToUse = part.partId || part.id;
    if (!partIdToUse) {
      this.toastr.error('Part ID is missing');
      return;
    }

    this.favoritesService.toggleFavorite(partIdToUse).subscribe({
      next: (res) => {
        if (res.isFavorite) {
          this.favoriteIds = [...this.favoriteIds, partIdToUse];
          this.toastr.success('Added to favorites');
        } else {
          this.favoriteIds = this.favoriteIds.filter(id => id !== partIdToUse);
          this.toastr.info('Removed from favorites');
        }
      },
      error: (err) => this.toastr.error('Failed to toggle favorite')
    });
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.filter-item')) {
      this.closeAllDropdowns();
    }
  }

  closeAllDropdowns(): void {
    for (const key in this.dropdownState) {
      this.dropdownState[key] = false;
    }
  }

  ngAfterViewInit() {
    window.addEventListener('scroll', this.checkVisibility.bind(this));
  }

  loadDropdowns(): void {
    this.dropdownService.getCars().subscribe({
      next: (data) => (this.cars = data),
      error: (err) => console.error('Error loading cars:', err),
    });

    this.dropdownService.getCategories().subscribe({
      next: (data) => (this.categories = data),
      error: (err) => console.error('Error loading categories:', err),
    });

    this.dropdownService.getParts().subscribe({
      next: (data) => (this.parts = data),
      error: (err) => console.error('Error loading parts:', err),
    });
  }

  onCarChange(): void {
    if (this.selectedCarId) {
      this.dropdownService.getModels(this.selectedCarId).subscribe({
        next: (data) => {
          this.models = data;
        },
        error: (err) => {
          console.error('Error fetching models:', err);
          this.models = [];
        },
      });
    } else {
      this.models = [];
    }
  }

  loadVehicleTypes(): void {
    this.dropdownService.getVehicleTypes().subscribe({
      next: (data) => {
        this.types = data;
      },
      error: (err) => {
        console.error('Error loading vehicle types:', err);
        this.types = [];
      },
    });
  }

  applyFilter(): void {

    const params: any = {};
    if (this.selectedCarId) params.carId = this.selectedCarId;
    if (this.selectedCategoryId) params.categoryId = this.selectedCategoryId;
    if (this.selectedPartId) params.partId = this.selectedPartId;
    if (this.selectedModelId) params.modelId = this.selectedModelId;
    if (this.selectedTypeId) params.typeId = this.selectedTypeId;

    this.dropdownService.filterParts(params).subscribe({
      next: (data) => {
        this.filteredParts = data;
        setTimeout(() => this.checkVisibility(), 300);
      },
      error: (err) => {
        console.error('Error fetching filtered parts:', err);
        this.filteredParts = [];
      },
    });
  }

  resetFilter(): void {
    this.selectedCarId = null;
    this.selectedCategoryId = null;
    this.selectedPartId = null;
    this.selectedModelId = null;
    this.selectedTypeId = null;
    this.filteredParts = [];
    this.showVehicleTypeDropdown = false;
    this.loadDropdowns();
  }

  openProductModal(part: any) {
    this.selectedPart = { ...part, quantity: this.selectedPart?.quantity || 1 };
  }

  addToCart(part: any, event: MouseEvent): void {
    const token = this.authService.getTokenUser();

    if (!token) {
      this.toastr.warning('Please login to add item to cart.', 'Not logged in');
      this.router.navigate(['/login']);
      return;
    }

    const quantity = part.quantity || 1;
    const partIdToUse = part.partId || part.id;

    if (!partIdToUse) {
      this.toastr.error('Part ID is missing');
      return;
    }

    this.cartService.addToCart(partIdToUse, quantity).subscribe({
      next: (res) => {
        this.toastr.success(`${part.name} added to cart`, 'Success');
        this.cartService.loadCartItems();  // Refresh cart items across components
        if (this.selectedPart) {
          this.flyToCartFromModal(MyConfig.api_address + part.partImage);
          this.closeProductModal();
        } else {
          this.flyToCart(event);
        }
      },
      error: (err) => {
        console.error('Add to cart failed: ', err);
        this.toastr.error('Could not add to cart', 'Error');
      }
    });
  }

  increaseQuantity() {
    if (this.selectedPart && this.selectedPart.quantity < 10) {
      this.selectedPart.quantity++;
    }
  }

  closeProductModal() {
    this.selectedPart = null;
  }

  decreaseQuantity() {
    if (this.selectedPart && this.selectedPart.quantity > 1) {
      this.selectedPart.quantity--;
    }
  }

  checkVisibility(): void {
    const cards = this.el.nativeElement.querySelectorAll('.product-card');

    cards.forEach((card: HTMLElement) => {
      this.renderer.addClass(card, 'visible');
    });
  }

  toggleDropdown(key: string) {
    Object.keys(this.dropdownState).forEach(k => {
      this.dropdownState[k] = k === key ? !this.dropdownState[k] : false;
    });
  }

  getSelectedCarName() {
    const car = this.cars.find(c => c.id === this.selectedCarId);
    return car ? car.name : 'Select Car';
  }

  selectCar(carId: number) {
    this.selectedCarId = carId;
    this.selectedModelId = null;
    this.models = [];
    this.toggleDropdown('car');
    this.onCarChange();
  }

  getSelectedModelName() {
    if (!this.selectedCarId) return 'Select Model';
    const model = this.models.find(m => m.id === this.selectedModelId);
    return model ? model.name : 'Select Model';
  }

  selectModel(modelId: number) {
    this.selectedModelId = modelId;
    this.toggleDropdown('model');
  }

  getSelectedCategoryName() {
    const category = this.categories.find(c => c.id === this.selectedCategoryId);
    return category ? category.name : 'Select Category';
  }

  selectCategory(categoryId: number) {
    this.selectedCategoryId = categoryId;
    this.toggleDropdown('category');

    if (categoryId === this.frontCategoryId) {
      this.showVehicleTypeDropdown = true;
      this.loadVehicleTypes();
    } else {
      this.showVehicleTypeDropdown = false;
      this.types = [];
      this.selectedTypeId = null;
    }
  }

  getSelectedPartName() {
    const part = this.parts.find(p => p.id === this.selectedPartId);
    return part ? part.name : 'Select Part';
  }

  selectPart(partId: number) {
    this.selectedPartId = partId;
    this.toggleDropdown('part');
  }

  getSelectedTypeName() {
    const type = this.types.find(t => t.id === this.selectedTypeId);
    return type ? type.name : 'Select Type';
  }

  selectType(typeId: number) {
    this.selectedTypeId = typeId;
    this.toggleDropdown('type');
  }

  flyToCart(event: MouseEvent) {
    const image = (event.target as HTMLElement).closest('.part-card')?.querySelector('.product-main-image') as HTMLImageElement;
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
    if (!cart) return;

    const imgClone = document.createElement('img');
    imgClone.src = imageUrl;

    const modalImage = document.querySelector('.modal-product-image') as HTMLElement;
    const imgRect = modalImage?.getBoundingClientRect();
    const cartRect = cart.getBoundingClientRect();

    if (!imgRect) return;

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