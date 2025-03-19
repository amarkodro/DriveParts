import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {PartsService} from '../services/parts.service';

@Component({
  selector: 'app-view-parts',
  templateUrl: './view-parts.component.html',
  styleUrl: './view-parts.component.css'
})
export class ViewPartsComponent implements OnInit {
  parts: any[]=[];
  category: string = '';
  selectedProduct: any;

  constructor(private route : ActivatedRoute, private partsService : PartsService) {}
    ngOnInit(): void {
      this.route.queryParams.subscribe(params => {
        this.category = params['category'] || 'all';
        this.loadParts();
      });

    }


  private loadParts() {
    if (this.category === 'featured') {
      this.partsService.getFeaturedParts().subscribe(data => this.parts = data);
    } else if (this.category === 'on-sale') {
      this.partsService.getOnSaleParts().subscribe(data => this.parts = data);
    } else if (this.category === 'new-arrivals') {
      this.partsService.getNewArrivalParts().subscribe(data => this.parts = data);
    }
  }

  openProductModal(part: any): void {
    console.log("Select product: ", part)
    this.selectedProduct = part;
    this.selectedProduct = { ...part, quantity: this.selectedProduct?.quantity || 1 };
  }

  /**
   * Closes the product detail modal
   */
  closeProductModal(): void {
    this.selectedProduct = null;
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

  addToCart(selectedProduct: any) {

  }


}
