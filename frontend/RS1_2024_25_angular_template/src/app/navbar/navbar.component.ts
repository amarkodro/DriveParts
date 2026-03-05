import {MyConfig} from '../my-config';
import { Component, HostListener, OnInit, ElementRef } from '@angular/core';
import { PartService } from '../services/navbar-search.service';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth-services/auth.service';
import { CartService } from '../services/cart.service';
import { Subscription } from 'rxjs';
import { NgZone } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
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
  cartItems: any[] = [];
  private userInfoSub!: Subscription;
  isListening = false;
  recognition: any;
  speechSupported = false;
  finalTranscript = '';
  isAdmin: boolean = false;

  constructor(private partService: PartService,
    private router: Router,
    private elementRef: ElementRef,
    private authService: AuthService,
    private cartService: CartService,
    private ngZone: NgZone,
    private cdRef: ChangeDetectorRef
  ) { }

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
            : MyConfig.api_address + '/' + user.imageUrl;
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
        this.isAdmin = userInfo?.role === 'Admin' || false;
        this.cartService.loadCartItems();
      } else {
        this.userName = null;
        this.isAdmin = false;
        this.cartItems = [];
      }
    });


    this.authService.userInfo$.subscribe((user) => {
      if (user) {
        this.userName = `${user.name} ${user.surname}`;
        this.userProfileImage = MyConfig.api_address + '/' + user.imageUrl;
        this.isLoggedIn = true;

        this.cartService.loadCartItems();
      } else {
        this.userName = '';
        this.userProfileImage = '';
        this.isLoggedIn = false;
        this.cartItems = [];
      }
    });


    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items.map(item => ({
        partId: item.partId,
        name: item.partName || item.name,
        quantity: item.quantity,
        price: item.price,
        image: MyConfig.api_address + (item.image || '/images/placeholder.png'),
      }));
    });

    // Debug token info
    const info = this.authService.getUserInfoFromToken();
    this.isAdmin = info?.role === 'Admin' || false;
    this.checkSpeechSupport();
  }
  checkSpeechSupport() {
    this.speechSupported = 'webkitSpeechRecognition' in window || 'SpeechRecognition' in window;
  }
  toggleVoiceSearch(event: Event) {
    event.stopPropagation(); // Prevent event bubbling

    if (!this.speechSupported) {
      alert('Speech recognition not supported in this browser. Try Chrome or Edge.');
      return;
    }

    if (this.isListening) {
      this.stopVoiceRecognition();
    } else {
      this.startVoiceRecognition();
    }
  }
  startVoiceRecognition() {
    if (this.isListening) return;

    this.isListening = true;
    this.finalTranscript = '';
    this.searchString = '';
    this.filteredParts = []; // Clear previous results

    const SpeechRecognition = (window as any).webkitSpeechRecognition || (window as any).SpeechRecognition;

    if (!this.recognition) {
      this.recognition = new SpeechRecognition();
      this.recognition.continuous = false;
      this.recognition.interimResults = true;
      this.recognition.lang = 'en-US';

      this.recognition.onresult = (event: any) => {
        let interimTranscript = '';

        for (let i = event.resultIndex; i < event.results.length; ++i) {
          if (event.results[i].isFinal) {
            this.finalTranscript += event.results[i][0].transcript;
          } else {
            interimTranscript += event.results[i][0].transcript;
          }
        }

        this.ngZone.run(() => {
          // Update search string for display
          this.searchString = this.finalTranscript + interimTranscript;

          // DIRECTLY UPDATE SEARCH RESULTS
          this.updateSearchResults(this.searchString);
        });
      };


      this.recognition.onerror = (event: any) => {
        console.error('Speech recognition error', event.error);
        this.stopVoiceRecognition();
      };

      this.recognition.onend = () => {
        this.ngZone.run(() => {
          this.isListening = false;
        });
      };
    }

    this.recognition.start();
  }
  updateSearchResults(searchText: string) {
    const normalizedSearch = searchText.toLowerCase().trim();

    if (normalizedSearch === '') {
      this.filteredParts = [];
      return;
    }

    // Split search into individual words
    const searchTerms = normalizedSearch.split(/\s+/).filter(term => term.length > 0);

    // Filter parts - search in both name and description
    this.filteredParts = this.allParts.filter(part => {
      const partName = part.name ? part.name.toLowerCase() : '';
      const partDesc = part.description ? part.description.toLowerCase() : '';

      // Check if all search terms appear in either name or description
      return searchTerms.every(term =>
        partName.includes(term) ||
        partDesc.includes(term)
      );
    });

    // Force UI update
    this.cdRef.detectChanges();
  }
  private triggerSearchUpdate() {
    // Create a shallow copy of the array to force change detection
    this.filteredParts = [...this.filteredParts];

    // Force Angular to run change detection
    this.cdRef.detectChanges();
  }
  stopVoiceRecognition() {
    if (this.recognition) {
      this.recognition.stop();
    }
    this.isListening = false;
  }

  ngOnDestroy(): void {
    this.stopVoiceRecognition();
    this.userInfoSub?.unsubscribe();
  }



  closeAllDropdowns() {
    this.dropdownOpen = false;
    this.cartOpen = false;
    this.menuOpen = false;
  }

  /*onSearchChange() {
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
  }*/
  onSearchChange() {
    this.updateSearchResults(this.searchString);
  }
  selectPart(part: any) {
    this.searchString = '';
    this.filteredParts = [];
    this.searchOpen = false;
    this.router.navigate(['/part-detail', part.partId]);
  }

  logout() {

    const userId = this.authService.getUserId();

    localStorage.removeItem(`promoCodeId-${userId}`);
    localStorage.removeItem(`usedCode-${userId}`);
    localStorage.removeItem(`discount-${userId}`);
    localStorage.removeItem(`supplierId-${userId}`);
    localStorage.removeItem(`paymentId-${userId}`);


    localStorage.removeItem('jwtToken');
    sessionStorage.removeItem('jwtToken');
    localStorage.removeItem('my-auth-token'); // Sync token cleanup

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
    if (!this.isLoggedIn) {
      this.router.navigate(['/login']);
      return;
    }
    if (!this.cartOpen) this.closeAllDropdowns();
    this.cartOpen = !this.cartOpen;
  }

  loadCartItems(): void {
    this.cartService.getCartItems().subscribe({
      next: (items: any[]) => {

        this.cartItems = items.map(item => ({
          partId: item.partId,
          name: item.partName || item.name,
          quantity: item.quantity,
          price: item.price,
          image: MyConfig.api_address + (item.image || '/images/placeholder.png'),
        }));
      },
      error: (err: any) => {
        console.error('Error loading cart:', err);
      }
    });
  }

  removeFromCart(partId: number) {
    this.cartService.removeItemFromCart(partId).subscribe({
      next: () => this.cartService.loadCartItems(),
      error: (err) => console.error('Error deleting from cart:', err)
    });
  }

  clearEntireCart() {
    this.cartService.clearCart().subscribe({
      next: () => this.loadCartItems(),
      error: err => console.error('Error clearing cart:', err)
    });
  }

  openSettings() {
    this.router.navigate(['/edit-profile']);
  }
}