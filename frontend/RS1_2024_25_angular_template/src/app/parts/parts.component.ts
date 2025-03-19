import {Component, OnInit, AfterViewInit, ElementRef, Renderer2, HostListener} from '@angular/core';
import { DropdownService } from '../services/dropdown.service';
import { Router } from '@angular/router';

interface DropdownItem {
  id: number;
  name: string;
}

interface Part {
  id: number;
  name: string;
  price: number;
  description: string;
  partImage: string;
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

  constructor(
    private dropdownService: DropdownService,
    private router: Router,
    private renderer: Renderer2,
    private el: ElementRef
  ) {}

  ngOnInit(): void {
    this.loadDropdowns();
  }

  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    // Provjeri je li klik izvan dropdown-a
    if (!this.el.nativeElement.contains(event.target)) {
      // Zatvori sve dropdown-ove
      this.dropdownState = {
        car: false,
        model: false,
        category: false,
        part: false,
        type: false
      };
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
        setTimeout(() => this.checkVisibility(), 300); // Dodajemo odgodu kako bi animacija radila na novim karticama
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

  addToCart(part: any) {}

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
    this.selectedModelId = null; // Resetuje model kad se auto promijeni
    this.models = []; // Reset modela
    this.toggleDropdown('car');
    this.onCarChange(); // Učitava modele na osnovu odabranog auta
  }


  getSelectedModelName() {
    if (!this.selectedCarId) return 'Select Model'; // Ako auto nije odabran, vraća default tekst
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
      this.types = []; // Reset tipova
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
}
