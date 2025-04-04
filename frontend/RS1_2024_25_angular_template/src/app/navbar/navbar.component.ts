import { Component, HostListener, OnInit, ElementRef } from '@angular/core';
import { PartService } from '../services/navbar-search.service';
import { Router } from '@angular/router';
import {AuthService} from '../services/auth-services/auth.service';
import {CartService} from '../services/cart.service';


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
  userName: string | null = null;
  cartOpen = false;
  cartItems : any[] = [];



  constructor(private partService: PartService,
              private router: Router,
              private elementRef: ElementRef,
              private authService: AuthService,
              private cartService: CartService,
     ) {}

  ngOnInit(): void {
    this.partService.getAllParts().subscribe({
      next: (data) => {
        this.allParts = data;
      },
      error: (error) => {
        console.error('Error fetching parts:', error);
      }
    });


    const token = this.authService.getTokenUser();
    if (token) {
      this.isLoggedIn = true;

      this.authService.getUserProfile().subscribe({
        next: (user) => {
          this.userName = `${user.name} ${user.surname}`;
          this.userProfileImage = user.imageUrl?.startsWith('http')
            ? user.imageUrl
            : 'http://localhost:7000/' + user.imageUrl;
        },
        error: () => {
          this.userName = 'User';
          this.userProfileImage = 'assets/user.png';
        }
      });

      this.cartService.loadCartItems();
    }


    this.authService.loginStatus$.subscribe((status: boolean) => {
      this.isLoggedIn = status;

      if (status) {
        const userInfo = this.authService.getUserInfoFromToken();
        this.userName = userInfo ? `${userInfo.name} ${userInfo.surname}` : 'User';
        this.cartService.loadCartItems();
      } else {
        this.userName = null;
        this.cartItems = [];
      }
    });


    this.authService.userInfo$.subscribe((user) => {
      if (user) {
        this.userName = `${user.name} ${user.surname}`;
        this.userProfileImage = user.imageUrl?.startsWith('http')
          ? user.imageUrl
          : 'http://localhost:7000/' + user.imageUrl;
      }
    });


    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items.map(item => ({
        partId: item.partId,
        name: item.partName || item.name,
        quantity: item.quantity,
        price: item.price,
        image: 'http://localhost:7000' + (item.image || '/images/placeholder.png'),
      }));
    });

    // Debug token info
    const info = this.authService.getUserInfoFromToken();
    console.log('Data from tokens', info);
  }



  closeAllDropdowns() {
    this.dropdownOpen = false;
    this.cartOpen = false;
    this.menuOpen = false;
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
    localStorage.removeItem('jwtToken');
    sessionStorage.removeItem('jwtToken');

    this.isLoggedIn = false;
    this.userName = null;
    this.dropdownOpen = false;

    this.router.navigate(['/login']).then(() => {
      window.location.reload();
    });
  }

  toggleDropdown() {
    if (!this.dropdownOpen) this.closeAllDropdowns();
    this.dropdownOpen = !this.dropdownOpen;
  }

  toggleMenu() {
    if (!this.menuOpen) this.closeAllDropdowns();
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
    const clickedInside = this.elementRef.nativeElement.contains(event.target);
    const target = event.target as HTMLElement;

    const isLogoClick = target.closest('.navbar-brand');
    const isNavLinkClick = target.closest('.nav-link');
    const isSearchClick = target.closest('.search-container');

    if (!clickedInside || isLogoClick || isNavLinkClick || isSearchClick) {
      this.dropdownOpen = false;
      this.menuOpen = false;
      this.searchOpen = false;
      this.cartOpen = false;
    }
  }

  getCartTotal() {
    return this.cartItems.reduce((sum, item) => sum + item.quantity * item.price, 0);
  }

  goToCart() {
    this.router.navigate(['/cart']);
  }

  toggleCartDropdown() {
    if(!this.isLoggedIn)
    {
      this.router.navigate(['/login']);
      return;
    }
    if (!this.cartOpen) this.closeAllDropdowns();
    this.cartOpen = !this.cartOpen;
  }

  loadCartItems(): void {
    this.cartService.getCartItems().subscribe({
      next: (items: any[]) => {
        console.log('API ANSWER: ',items);

        this.cartItems = items.map(item => ({
          partId: item.partId,
          name: item.partName || item.name,
          quantity: item.quantity,
          price: item.price,
          image: 'http://localhost:7000' + (item.image || '/images/placeholder.png'),
        }));
      },
      error: (err: any) => {
        console.error('Error loading cart:', err);
      }
    });
  }

  removeFromCart(partId:number) {
    this.cartService.removeItemFromCart(partId).subscribe({
      next: () => this.cartService.loadCartItems(),
      error: (err) => console.error('Error deleting from cart:', err)
    });
  }

  clearEntireCart(){
    this.cartService.clearCart().subscribe({
      next: () => this.loadCartItems(),
      error: err => console.error('Error clearing cart:', err)
    });
  }

}
