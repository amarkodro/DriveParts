import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { loadStripe } from '@stripe/stripe-js';
import { StripeService } from '../services/stripe.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth-services/auth.service';
import { CitiesService } from '../services/cities.service';
import { UserService } from '../services/user.service';
import { SuppliersService } from '../services/suppliers.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  checkoutForm!: FormGroup;
  cartItems: any[] = [];
  discount: number = 0;
  total: number = 0;
  usedCode: string = '';
  isPlacingOrder: boolean = false;
  isEditable: boolean = false;
  cities: any[] = [];
  selectedCityId: number | null = null;
  dropdownState: { [key: string]: boolean } = {
    city: false,
    supplier: false,
  };
  phoneExists: boolean = false;
  promoCodeId: number | null = null;
  suppliers: any[] = [];
  selectedSupplierId: number | null = null;
  selectedPaymentMethod: number | null = null;
  triedSubmit: boolean = false;
  currentUser: any = null;


  constructor(private fb: FormBuilder,
    private router: Router,
    private cartService: CartService,
    private stripeService: StripeService,
    private toastr: ToastrService,
    private authService: AuthService,
    private cityService: CitiesService,
    private userService: UserService,
    private supplierService: SuppliersService
  ) { }

  ngOnInit(): void {
    this.cityService.getCity().subscribe({
      next: (data) => {
        this.cities = data;
      },
      error: (error) => {
        console.error('Error loading cities', error);
      }

    })

    this.loadSuppliers();

    const userId = this.authService.getUserId();
    this.promoCodeId = this.cartService.getPromoCodeId(userId);

    this.usedCode = this.cartService.getUsedCode(userId);
    this.checkoutForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required,
      Validators.pattern(/^\+387 6\d \d{3} \d{3,4}$/)
      ]],
      address: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', Validators.required],
      country: ['', Validators.required],
    });

    this.checkoutForm.get('phoneNumber')?.valueChanges.subscribe(phone => {
      if (phone && phone.length >= 6) {
        this.authService.checkPhone(phone, userId).subscribe({
          next: (res: any) => {
            this.phoneExists = res.exists;
          },
          error: (err) => {
            console.error('Error checking phone:', err);
          }
        });
      } else {
        this.phoneExists = false;
      }
    });



    this.authService.getUserProfile().subscribe({
      next: user => {
        this.currentUser = user;

        this.checkoutForm.patchValue({
          fullName: `${user.name} ${user.surname}`,
          email: user.email,
          phoneNumber: user.phoneNumber || '',
          address: user.address || '',
        });

        if (user.cityId) {
          this.cityService.getCityWithCountry(user.cityId).subscribe({
            next: cityData => {
              this.checkoutForm.patchValue({
                city: cityData.name,
                postalCode: cityData.postalCode,
                country: cityData.countryName,
              });
              this.selectedCityId = user.cityId;
            }
          });
        }

        if (!user.phoneNumber || !user.address || !user.cityId) {
          this.isEditable = true;
          this.toastr.info("Please enter the missing information before ordering.");
        }
      },
      error: err => {
        console.error("Error loading user profile:", err);
      }
    });


    this.cartService.cartItems$.subscribe(items => {
      const activeCartItems = items.filter(item => !item.isSavedForLater);
      this.cartItems = activeCartItems;
      this.total = activeCartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);
      this.discount = this.cartService.getDiscount(userId);
    });
  }

  async placeOrder() {
    this.triedSubmit = true;

    this.checkoutForm.markAllAsTouched();

    if (this.checkoutForm.invalid || !this.selectedSupplierId || !this.selectedPaymentMethod) {
      this.toastr.info("Please enter the missing information before ordering.");
      this.isPlacingOrder = false;
      return;
    }

    if (this.phoneExists) {
      this.toastr.error("This phone number is already registered.");
      this.isPlacingOrder = false;
      return;
    }

    this.isPlacingOrder = true;

    const user = this.currentUser;
    const userId = this.authService.getUserId();

    const fullName = this.checkoutForm.get('fullName')?.value || '';
    const [name, surname] = fullName.split(' ');

    const updatedUser = {
      name: name || user?.name || '',
      surname: surname || user?.surname || '',
      email: user?.email || '',
      phoneNumber: this.checkoutForm.get('phoneNumber')?.value || user?.phoneNumber || '',
      address: this.checkoutForm.get('address')?.value || user?.address || '',
      cityId: this.selectedCityId || user?.cityId || 0,
      username: user?.username || '',
      is2FActive: user?.is2FActive || false
    };


    if (!updatedUser.username || !updatedUser.email) {
      this.toastr.error("Invalid user data. Please log out and log in again.");
      return;
    }

    try {
      await this.userService.updateInfo(userId, updatedUser).toPromise();
      this.toastr.info("User info updated successfully.");
    } catch (error) {
      console.error("User update failed:", error);
      this.toastr.error("Failed to update your profile.");
      this.isPlacingOrder = false;
      return;
    }

    const discount = this.discount ?? 0;
    const subtotal = this.cartItems.reduce((acc, item) => acc + item.price * item.quantity, 0);

    if (discount >= subtotal) {
      console.error("Discount cannot be greater than or equal to the total amount!");
      this.isPlacingOrder = false;
      return;
    }

    const discountRatio = discount / subtotal;

    const items = this.cartItems.map(item => {
      const itemTotal = item.price * item.quantity;
      const discountedTotal = itemTotal - (itemTotal * discountRatio);
      const unitPrice = discountedTotal / item.quantity;

      return {
        name: item.partName,
        quantity: item.quantity,
        price: Math.round(unitPrice * 100)
      };
    });

    localStorage.setItem(`paymentId-${userId}`, this.selectedPaymentMethod!.toString());

    setTimeout(async () => {
      try {
        if (this.selectedPaymentMethod === 1) {
          await this.stripeService.redirectToCheckout(items);
          localStorage.removeItem('discount');
        }
        else if (this.selectedPaymentMethod === 2) {
          this.router.navigate(['/order-success']);
        }

      } catch (error) {
        console.error('Payment failed:', error);
      } finally {
        this.isPlacingOrder = false;
      }
    }, 2000);
  }

  toggleEdit() {
    this.isEditable = !this.isEditable;
  }

  toggleDropdown(dropdownName: string) {
    this.dropdownState[dropdownName] = !this.dropdownState[dropdownName];
  }

  selectCity(city: any) {
    this.selectedCityId = city.id;
    this.checkoutForm.patchValue({
      city: city.name,
      postalCode: city.postalCode,
      country: city.countryName
    });
    this.dropdownState['city'] = false;

  }

  getSelectedCityName() {
    return this.checkoutForm.get('city')?.value || 'Select City';
  }

  @HostListener('document:click', ['$event'])
  handleOutsideClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const clickedInsideDropdown = target.closest('.dropdown');

    if (!clickedInsideDropdown) {
      Object.keys(this.dropdownState).forEach(key => {
        this.dropdownState[key] = false;
      });
    }
  }

  loadSuppliers() {
    this.supplierService.getAllSuppliers().subscribe({
      next: (data) => (this.suppliers = data),
    })
  }

  getSelectedSupplierName(): string {
    const selected = this.suppliers.find(item => item.supplierId === this.selectedSupplierId);
    return selected ? selected.name : 'Select supplier';
  }

  selectSupplier(supplier: any): void {
    this.selectedSupplierId = supplier.supplierId;
    this.dropdownState['supplier'] = false;
    localStorage.setItem(`supplierId-${this.authService.getUserId()}`, supplier.supplierId.toString());
  }

}
