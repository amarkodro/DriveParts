import { Component, OnInit } from '@angular/core';
import { PartService } from '../services/navbar-search.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  searchString: string = '';
  allParts: any[] = [];
  filteredParts: any[] = [];

  constructor(private partService: PartService, private router: Router) {}

  ngOnInit() {
    this.partService.getAllParts().subscribe(
      (data) => {
        this.allParts = data;
        console.log('Fetched all parts:', this.allParts); 
      },
      (error) => {
        console.error('Error fetching parts:', error);
      }
    );
  }

  onSearchChange() {
    const normalizedSearch = this.searchString.toLowerCase().trim();
    console.log('Search term:', normalizedSearch); 
    if (normalizedSearch === '') {
      this.filteredParts = [];
      console.log('No search term, filteredParts cleared');
    } else {
      this.filteredParts = this.allParts.filter(part =>
        part.description.toLowerCase().includes(normalizedSearch)
      );
      console.log('Filtered parts:', this.filteredParts); 
    }
  }

  selectPart(part: any) {
    console.log('Selected part:', part); 
    this.searchString = '';
    this.filteredParts = [];
    this.router.navigate(['/part-detail', part.partId]);
  }
}
