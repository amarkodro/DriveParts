import { Component, HostListener, OnInit, ElementRef } from '@angular/core';
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
  dropdownOpen: boolean = false;
  isScrolled: boolean = false;
  userProfileImage: string = 'assets/default-user.png';
  isLoggedIn: boolean = false;
  menuOpen: boolean = false;
  searchOpen: boolean = false;

  constructor(private partService: PartService, private router: Router, private elementRef: ElementRef) {}

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
      this.searchOpen = false;
    } else {
      this.filteredParts = this.allParts.filter(part =>
        part.description.toLowerCase().includes(normalizedSearch)
      );
      this.searchOpen = this.filteredParts.length > 0;
    }
  }

  selectPart(part: any) {
    this.searchString = '';
    this.filteredParts = [];
    this.searchOpen = false;
    this.router.navigate(['/part-detail', part.partId]);
  }

  logout() {
    this.isLoggedIn = false;
    this.dropdownOpen = false;
  }

  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
  }

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }
  closeMenu(): void {
    this.menuOpen = false;
  }
  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.isScrolled = window.scrollY > 90;
  }


  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.dropdownOpen = false;
      this.menuOpen = false;
      this.searchOpen = false;
    }
  }
}
