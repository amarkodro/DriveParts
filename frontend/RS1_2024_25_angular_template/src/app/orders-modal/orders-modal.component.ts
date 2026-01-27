import { Component, Input, HostListener } from '@angular/core';

@Component({
  selector: 'app-orders-modal',
  templateUrl: './orders-modal.component.html',
  styleUrls: ['./orders-modal.component.css']
})
export class OrdersModalComponent {
  @Input() customerName: string = '';
  @Input() orders: any[] = [];
  @Input() loading: boolean = false;
  
  isOpen: boolean = false;

  open(): void {
    this.isOpen = true;
    document.body.style.overflow = 'hidden'; // Prevent scrolling
  }

  close(): void {
    this.isOpen = false;
    document.body.style.overflow = ''; // Restore scrolling
  }

  // Close modal when clicking outside
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const modal = document.querySelector('.modal-content');
    const overlay = document.querySelector('.modal-overlay');
    
    if (this.isOpen && overlay && modal && 
        event.target === overlay && !modal.contains(event.target as Node)) {
      this.close();
    }
  }

  // Close modal on ESC key
  @HostListener('document:keydown.escape', ['$event'])
  onKeydownHandler(event: KeyboardEvent): void {
    if (this.isOpen) {
      this.close();
    }
  }
}