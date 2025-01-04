import { Component, OnInit } from '@angular/core';
import { DropdownService } from '../services/dropdown.service';

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
export class PartsComponent implements OnInit {
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

  showVehicleTypeDropdown: boolean = false;
  frontCategoryId: number = 11;

  constructor(private dropdownService: DropdownService) {}

  ngOnInit(): void {
    this.loadDropdowns();
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

  onCategoryChange(): void {
    console.log('Selected Category ID:', this.selectedCategoryId);
    console.log('Front Category ID:', this.frontCategoryId);

    if (Number(this.selectedCategoryId) === this.frontCategoryId) {
      console.log('Front category selected, showing vehicle type dropdown');
      this.showVehicleTypeDropdown = true;
      this.loadVehicleTypes();
    } else {
      console.log('Non-Front category selected, hiding vehicle type dropdown');
      this.showVehicleTypeDropdown = false;
      this.types = []; //
      this.selectedTypeId = null;
    }
  }

  loadVehicleTypes(): void {
    this.dropdownService.getVehicleTypes().subscribe({
      next: (data) => {
        console.log('Loaded Vehicle Types:', data);
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
      },
      error: (err) => {
        console.error('Error fetching filtered parts:', err);
        this.filteredParts = [];
      },
    });
  }

  resetFilters(): void {
    this.selectedCarId = null;
    this.selectedCategoryId = null;
    this.selectedPartId = null;
    this.selectedModelId = null;
    this.selectedTypeId = null;

    this.filteredParts = [];

    this.showVehicleTypeDropdown = false;
    this.loadDropdowns();
  }
}
