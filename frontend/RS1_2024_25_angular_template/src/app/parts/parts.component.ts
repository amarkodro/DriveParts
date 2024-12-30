import { Component, OnInit } from '@angular/core';
import { DropdownService } from '../services/dropdown.service';
import { HttpParams } from '@angular/common/http';

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
  styleUrls: ['./parts.component.css']
})
export class PartsComponent implements OnInit {
  cars: DropdownItem[] = [];
  categories: DropdownItem[] = [];
  parts: DropdownItem[] = [];
  brands: DropdownItem[] = [];
  filteredParts: Part[] = [];

  selectedCarId: number | null = null;
  selectedCategoryId: number | null = null;
  selectedPartId: number | null = null;
  selectedBrandId: number | null = null;

  constructor(private dropdownService: DropdownService) {}

  ngOnInit(): void {
    this.loadDropdowns();
  }

  loadDropdowns(): void {
    this.dropdownService.getCars().subscribe({
      next: (data) => (this.cars = data),
      error: (err) => console.error('Error loading cars:', err)
    });

    this.dropdownService.getCategories().subscribe({
      next: (data) => (this.categories = data),
      error: (err) => console.error('Error loading categories:', err)
    });

    this.dropdownService.getParts().subscribe({
      next: (data) => (this.parts = data),
      error: (err) => console.error('Error loading parts:', err)
    });

    this.dropdownService.getBrands().subscribe({
      next: (data) => (this.brands = data),
      error: (err) => console.error('Error loading brands:', err)
    });
  }

  applyFilter(): void {
    // Priprema parametara - samo one koji nisu null
    const params: any = {};
    if (this.selectedCarId) params.carId = this.selectedCarId;
    if (this.selectedCategoryId) params.categoryId = this.selectedCategoryId;
    if (this.selectedPartId) params.partId = this.selectedPartId;
    if (this.selectedBrandId) params.manufacturerId = this.selectedBrandId;

    console.log('Filter params:', params);

    // Poziv servisa sa filtriranim parametrima
    this.dropdownService.filterParts(params).subscribe({
      next: (data) => {
        console.log('Filtered parts:', data);
        this.filteredParts = data;
      },
      error: (err) => {
        console.error('Error fetching filtered parts:', err);
        this.filteredParts = [];
      },
    });
  }

}
