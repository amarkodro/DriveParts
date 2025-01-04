import { Component, OnInit } from '@angular/core';
import { PartService } from '../services/navbar-search.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  searchString: string = '';
  allParts: any[] = [];
  filteredParts: any[] = [];

  constructor(private partService: PartService) {}

  ngOnInit() {
    this.partService.getAllParts().subscribe(
      (data) => {
        this.allParts = data;
      },
      (error) => {
        console.error('Error fetching parts:', error);
      }
    );
  }

  onSearchChange() {
    const normalizedSearch = this.searchString.toLowerCase().trim();
    if (normalizedSearch === '') {
      this.filteredParts = [];
    } else {
      this.filteredParts = this.allParts.filter(part =>
        part.description.toLowerCase().includes(normalizedSearch)
      );
    }
  }

  selectPart(part: any) {
    this.searchString = '';
    this.filteredParts = [];
  }
}
