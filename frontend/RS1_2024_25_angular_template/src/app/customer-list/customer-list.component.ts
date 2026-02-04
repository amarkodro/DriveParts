import { Component, HostListener, OnInit } from '@angular/core';
import { CustomerService } from '../services/customer.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {
  customers: any[] = [];
  paginatedCustomers: any[] = [];
  searchTerm = '';
  roleFilter = 'all';
  pageSize = 10;
  currentPage = 1;
  totalCount = 0;
  totalPages = 1;
  loading = false;

  showOrdersModal = false;
  selectedCustomerId: number | null = null;
  selectedCustomerName = '';
  orders: any[] = [];
  ordersLoading = false;

  private searchSubject = new Subject<string>();

  constructor(
    private customerService: CustomerService
  ) { }

  ngOnInit(): void {
    // Set up debounce for search input
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(() => {
      this.currentPage = 1;
      this.loadCustomers();
    });

    this.loadCustomers();
  }

  onSearchChange(): void {
    this.searchSubject.next(this.searchTerm);
  }

  loadCustomers(): void {
    this.loading = true;
    this.customerService.getCustomers(
      this.searchTerm,
      this.roleFilter,
      this.currentPage,
      this.pageSize
    ).subscribe({
      next: (response) => {
        this.customers = response.items || response;
        this.totalCount = response.totalCount || response.length;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.updatePaginatedCustomers();
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  updatePaginatedCustomers(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedCustomers = this.customers.slice(startIndex, endIndex);
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.loadCustomers();
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.loadCustomers();
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePaginatedCustomers();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePaginatedCustomers();
    }
  }

  openOrdersModal(customerId: number, customerName: string): void {
    this.selectedCustomerId = customerId;
    this.selectedCustomerName = customerName;
    this.showOrdersModal = true;
    this.loadCustomerOrders(customerId);
    document.body.style.overflow = 'hidden'; // Prevent background scrolling
  }

  closeOrdersModal(): void {
    this.showOrdersModal = false;
    this.selectedCustomerId = null;
    this.selectedCustomerName = '';
    this.orders = [];
    document.body.style.overflow = ''; // Restore scrolling
  }

  loadCustomerOrders(customerId: number): void {
    this.ordersLoading = true;
    this.customerService.getCustomerOrders(customerId).subscribe({
      next: (orders) => {
        this.orders = orders;
        this.ordersLoading = false;
      },
      error: () => {
        this.ordersLoading = false;
      }
    });
  }



  // Close modal on ESC key
  @HostListener('document:keydown.escape', ['$event'])
  onKeydownHandler(event: KeyboardEvent): void {
    if (this.showOrdersModal) {
      this.closeOrdersModal();
    }
  }

  // Image enlarge functionality
  enlargedImage: string | null = null;

  enlargeImage(imageUrl: string): void {
    this.enlargedImage = imageUrl;
  }

  closeEnlargedImage(): void {
    this.enlargedImage = null;
  }

  handleImageError(event: any): void {
    event.target.src = 'assets/default-user.png';
  }

  // Suggestions logic
  suggestions: string[] = [];

  onSearchInput(): void {
    // Original search change logic (debounced)
    this.searchSubject.next(this.searchTerm);

    // Fetch suggestions directly if term is long enough
    if (this.searchTerm.length >= 2) {
      this.customerService.getCustomerSuggestions(this.searchTerm).subscribe(data => {
        this.suggestions = data;
      });
    } else {
      this.suggestions = [];
    }
  }

  selectSuggestion(suggestion: string): void {
    this.searchTerm = suggestion;
    this.suggestions = [];
    this.onSearchChange(); // Trigger search immediately
  }

  // Close suggestions when clicking outside
  @HostListener('document:click', ['$event'])
  clickout(event: MouseEvent): void {
    // Only close if click is outside search container
    // Note: The previous logic for modal closing handles one click listener, 
    // we can merge logic or just check target here as well.
    const target = event.target as HTMLElement;
    if (!target.closest('.search-container')) {
      this.suggestions = [];
    }

    // Existing modal logic
    const modal = document.querySelector('.modal-content');
    const overlay = document.querySelector('.modal-overlay');

    if (this.showOrdersModal && overlay && modal &&
      event.target === overlay && !modal.contains(event.target as Node)) {
      this.closeOrdersModal();
    }
  }
}