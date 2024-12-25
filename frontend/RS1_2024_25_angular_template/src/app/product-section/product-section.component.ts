import { Component, OnInit } from '@angular/core';
import { PartsService } from '../services/parts.service';

@Component({
  selector: 'app-product-section',
  templateUrl: './product-section.component.html',
  styleUrls: ['./product-section.component.css'],
})
export class ProductSectionComponent implements OnInit {
  featuredParts: any[] = [];
  newArrivalParts: any[] = [];
  onSaleParts: any[] = [];

  constructor(private partsService: PartsService) {}

  ngOnInit(): void {
    this.loadFeaturedParts();
    this.loadNewArrivalParts();
    this.loadOnSaleParts();
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
        console.error('Error fetching on sale parts:', err);
      },
    });
  }
}
