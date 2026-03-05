import { Component, OnInit, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { PartService, Part } from '../services/Adminpart.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-admin-parts',
  templateUrl: './admin-parts.component.html',
  styleUrls: ['./admin-parts.component.css']
})
export class AdminPartsComponent implements OnInit {
  searchTerm = '';
  categoryFilter: number | undefined;
  manufacturerFilter: number | undefined;
  minPrice: number | undefined;
  maxPrice: number | undefined;

  parts: Part[] = [];
  paginatedParts: Part[] = [];
  pageSize = 10;
  currentPage = 1;
  totalCount = 0;
  totalPages = 1;
  loading = false;

  categories: any[] = [];
  manufacturers: any[] = [];

  Math = Math; // Expose Math for template

  private searchSubject = new Subject<string>();

  constructor(private partService: PartService, private router: Router) { }

  ngOnInit(): void {
    // Set up debounce for search input
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(() => {
      this.currentPage = 1;
      this.loadParts();
    });

    this.loadCategories();
    this.loadManufacturers();
    this.loadParts();
  }

  loadCategories(): void {
    this.partService.getCategories().subscribe((data) => {
      this.categories = data;
    });
  }

  loadManufacturers(): void {
    this.partService.getManufacturers().subscribe((data) => {
      this.manufacturers = data;
    });
  }

  loadParts(): void {
    this.loading = true;
    this.partService.getParts(
      this.currentPage,
      this.pageSize,
      this.searchTerm || undefined,
      this.categoryFilter,
      this.manufacturerFilter,
      this.minPrice,
      this.maxPrice
    ).subscribe({
      next: (response) => {
        this.paginatedParts = response.items || response;
        this.totalCount = response.totalCount || response.length;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading parts:', err);
        this.loading = false;
      }
    });
  }

  get totalPagesArray(): number[] {
    return Array(this.totalPages).fill(0).map((_, i) => i + 1);
  }

  setPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadParts();
    }
  }

  onSearchChange(): void {
    this.searchSubject.next(this.searchTerm);
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.loadParts();
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.loadParts();
  }

  handleImageError(event: any): void {
    event.target.src = 'https://via.placeholder.com/50';
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.categoryFilter = undefined;
    this.manufacturerFilter = undefined;
    this.minPrice = undefined;
    this.maxPrice = undefined;
    this.currentPage = 1;
    this.loadParts();
  }

  deletePart(id: number): void {
    if (!id) {
      console.error('Invalid part ID:', id);
      return;
    }
    Swal.fire({
      title: 'Are you sure?',
      text: 'This part will be permanently deleted.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'Cancel'
    }).then((result) => {
      if (result.isConfirmed) {
        this.partService.deletePart(id).subscribe({
          next: () => {
            this.loadParts();
            Swal.fire('Deleted!', 'Part has been deleted.', 'success');
          },
          error: (err) => {
            console.error('Error deleting part:', err);
            Swal.fire('Error', 'Failed to delete part.', 'error');
          }
        });
      }
    });
  }

  openAddModal(): void {
    this.router.navigate(['/add']);
  }

  editPart(part: Part): void {
    this.router.navigate([`/put/${part.partId}`]);
  }

  // Image enlarge and zoom functionality
  enlargedImage: string | null = null;
  isZoomed = false;
  zoomOrigin = 'center';

  enlargeImage(imageUrl: string): void {
    this.enlargedImage = imageUrl;
    this.resetZoom();
  }

  toggleZoom(event: MouseEvent): void {
    if (!this.isZoomed) {
      const img = event.target as HTMLImageElement;
      const rect = img.getBoundingClientRect();
      const x = ((event.clientX - rect.left) / rect.width) * 100;
      const y = ((event.clientY - rect.top) / rect.height) * 100;
      this.zoomOrigin = `${x}% ${y}%`;
      this.isZoomed = true;
    } else {
      this.isZoomed = false;
    }
  }

  resetZoom(): void {
    this.isZoomed = false;
    this.zoomOrigin = 'center';
  }

  closeEnlargedImage(): void {
    this.enlargedImage = null;
    this.resetZoom();
  }

  // Suggestions logic
  suggestions: string[] = [];

  onSearchInput(): void {
    // Original search change (debounced table refresh)
    this.searchSubject.next(this.searchTerm);

    // Fetch suggestions directly if term is long enough
    if (this.searchTerm.length >= 2) {
      this.partService.getPartSuggestions(this.searchTerm).subscribe(data => {
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
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.search-container')) {
      this.suggestions = [];
    }
  }
}