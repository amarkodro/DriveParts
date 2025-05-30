import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PartService, Part } from '../services/Adminpart.service';

@Component({
  selector: 'app-admin-parts',
  templateUrl: './admin-parts.component.html',
  styleUrls: ['./admin-parts.component.css']
})
export class AdminPartsComponent implements OnInit {
 

  constructor(private partService: PartService, private router: Router) {}
 searchTerm = '';
  parts: Part[] = [];
  paginatedParts: Part[] = [];
  pageSize = 10;
  currentPage = 1;
 
  ngOnInit(): void {
    this.loadParts();
  }
// Get filtered parts based on search term
  get filteredParts(): Part[] {
    return this.parts.filter(part => 
      part.name.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }
    // Update total pages based on filtered results
  get totalPages(): number[] {
    const pageCount = Math.ceil(this.filteredParts.length / this.pageSize);
    return Array(pageCount).fill(0).map((_, i) => i + 1);
  }
  loadParts(): void {
    this.partService.getParts().subscribe((data) => {
      this.parts = data;
      this.setPage(1); // Start with page 1
    });
  }
  setPage(page: number): void {
    this.currentPage = page;
    const startIndex = (page - 1) * this.pageSize;
    this.paginatedParts = this.filteredParts.slice(startIndex, startIndex + this.pageSize);
  }
 
// Reset to first page when search term changes
  onSearchChange(): void {
    this.setPage(1);
  }
  deletePart(id: number): void {
    console.log('Attempting to delete part with ID:', id);  // Log ID to confirm it's correct
    if (!id) {
      console.error('Invalid part ID:', id);  // Log error if the ID is invalid
      return;
    }
    console.log('Deleting part with id:', id); // Check if ID is correct
    if (confirm('Are you sure you want to delete this part?')) {
      this.partService.deletePart(id).subscribe({
        next: () => {
          console.log('Part deleted successfully');
          this.parts = this.parts.filter(p => p.partId !== id);
          this.loadParts();
        },
        error: (err) => {
          console.error('Error deleting part:', err);
          alert('Failed to delete part');
        }
      });
    }
  }

  openAddModal(): void {
    this.router.navigate(['/add']); // Navigate to the "Add Part" form
  }

  editPart(part: Part): void {
    this.router.navigate([`/put/${part.partId}`]); // Navigate to the "Edit Part" form with part ID
  }
}
