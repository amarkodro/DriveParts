import { Component, OnInit } from '@angular/core';
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

  constructor(private partsService: PartsService, private router: Router) {}

  ngOnInit(): void {
    this.loadFeaturedParts();
    this.loadNewArrivalParts();
    this.loadOnSaleParts();
  }

  loadFeaturedParts(): void {
    this.partsService.getFeaturedParts().subscribe({
      next: (data) => {
        this.featuredParts = data;
        console.log('Featured Parts:', this.featuredParts);
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
        console.log('NewArrival Parts:', this.featuredParts);
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
        console.log('OnSale Parts:', this.featuredParts);
      },
      error: (err) => {
        console.error('Error fetching on sale parts:', err);
      },
    });
  }

  navigateToPartDetail(part: any): void {
    const partId = part.partId;
    if (!partId) {
      console.error('Part ID is undefined:', part);
      return;
    }
    console.log('Navigating to part detail with ID:', partId);
    this.router.navigate(['/part-detail', partId]);
  }
}
