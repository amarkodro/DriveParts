import { MyConfig } from '../my-config';
import { Component, OnInit, ViewChild, ElementRef, HostListener } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CitiesService } from '../services/cities.service';
import { AuthService } from '../services/auth-services/auth.service';
import { GenderService } from '../services/gender.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { debounceTime } from 'rxjs/operators';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  @ViewChild('dropdownWrapper', { static: true }) wrapperRef!: ElementRef;
  editProfileForm!: FormGroup;
  previewUrl: string | ArrayBuffer | null = null;
  genders: any[] = [];
  cities: any[] = [];
  selectedGenderId: number | null = null;
  selectedCityId: number | null = null;
  dropdownState: { [key: string]: boolean } = {
    gender: false,
    city: false,
  }
  selectedImageFile: File | null = null;
  apiUrl: string = MyConfig.api_address;
  originalUser: any;
  isSubmitting: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private http: HttpClient,
    private genderService: GenderService,
    private citiesService: CitiesService,
    private eRef: ElementRef,
    private userService: UserService,

  ) { }

  ngOnInit(): void {
    this.citiesService.getCity().subscribe(c => this.cities = c);
    this.genderService.getGenders().subscribe(g => this.genders = g);


    this.editProfileForm = this.fb.group({
      username: ['', [Validators.required]],
      name: ['', Validators.required],
      surname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\+387 6\d \d{3} \d{3,4}$/)]],
      address: ['', Validators.required],
      cityId: ['', Validators.required],
      genderId: ['', Validators.required],
    });

    const userId = this.authService.getUserId();


    this.authService.getUserProfile().subscribe({
      next: (user) => {
        this.originalUser = user;

        this.editProfileForm.patchValue({
          username: user.username,
          name: user.name,
          surname: user.surname,
          email: user.email,
          phoneNumber: user.phoneNumber,
          address: user.address,
          cityId: user.cityId,
          genderId: user.genderId,
        });
        this.selectedCityId = user.cityId;
        this.selectedGenderId = user.genderId;

        this.previewUrl = MyConfig.api_address + '/' + user.imageUrl;

      },
      error: err => {
        console.error('Error retrieving user:', err);
      }
    });


    this.editProfileForm.get('username')?.valueChanges
      .pipe(debounceTime(400))
      .subscribe(value => {
        if (value && this.originalUser && value !== this.originalUser.username) {
          this.authService.checkUsername(value).subscribe((res: any) => {
            if (res.exists) {
              this.editProfileForm.get('username')?.setErrors({ exists: true });
            }
          });
        }
      });

    this.editProfileForm.get('email')?.valueChanges
      .pipe(debounceTime(400))
      .subscribe(value => {
        if (value && this.originalUser && value !== this.originalUser.email) {
          this.authService.checkEmail(value).subscribe((res: any) => {
            if (res.exists) {
              this.editProfileForm.get('email')?.setErrors({ exists: true });
            }
          });
        }
      });

    this.editProfileForm.get('phoneNumber')?.valueChanges
      .pipe(debounceTime(400))
      .subscribe(value => {
        if (value && this.originalUser && value !== this.originalUser.phoneNumber) {
          this.authService.checkPhone(value).subscribe((res: any) => {
            if (res.exists) {
              this.editProfileForm.get('phoneNumber')?.setErrors({ exists: true });
            }
          });
        }
      });


  }


  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.handleFile(input.files[0]);
    }
  }

  onFileDropped(files: FileList): void {
    if (files && files.length > 0) {
      this.handleFile(files[0]);
    }
  }

  private handleFile(file: File): void {
    this.selectedImageFile = file;

    const reader = new FileReader();
    reader.onload = () => {
      this.previewUrl = reader.result;
    };
    reader.readAsDataURL(this.selectedImageFile);
  }

  triggerFileUpload(): void {
    this.fileInput.nativeElement.click();
  }

  toggleDropdown(type: string): void {
    this.dropdownState[type] = !this.dropdownState[type];
    Object.keys(this.dropdownState).forEach(key => {
      if (key !== type) this.dropdownState[key] = false;
    });
  }

  selectGender(id: number): void {
    this.selectedGenderId = id;
    this.editProfileForm.patchValue({ genderId: id });
    this.dropdownState['gender'] = false;
  }

  getSelectedGenderName(): string | null {
    const selected = this.genders.find(g => g.id === this.selectedGenderId);
    return selected ? selected.genderName : null;
  }

  selectCity(id: number): void {
    this.selectedCityId = id;
    this.editProfileForm.patchValue({ cityId: id });
    this.dropdownState['city'] = false;
  }

  getSelectedCityName(): string | null {
    const selected = this.cities.find(c => c.id === this.selectedCityId);
    return selected ? selected.name : null;
  }
  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    if (!this.wrapperRef.nativeElement.contains(event.target)) {
      this.dropdownState['city'] = false;
      this.dropdownState['gender'] = false;
    }
  }



  onSubmit(): void {

    this.editProfileForm.markAllAsTouched();

    if (this.editProfileForm.invalid) return;

    this.isSubmitting = true;

    setTimeout(() => {
      const formValues = this.editProfileForm.value;
      const formData = new FormData();

      formData.append('username', formValues.username);
      formData.append('name', formValues.name);
      formData.append('surname', formValues.surname);
      formData.append('email', formValues.email);
      formData.append('phoneNumber', formValues.phoneNumber);
      formData.append('address', formValues.address);
      formData.append('cityId', this.selectedCityId?.toString() || '');
      formData.append('genderId', this.selectedGenderId?.toString() || '');

      if (this.selectedImageFile) {
        formData.append('image', this.selectedImageFile);
      }

      const userId = this.authService.getUserId();

      this.userService.editUser(userId, formData).subscribe({
        next: (updatedUser) => {
          this.previewUrl = this.apiUrl + '/' + updatedUser.imageUrl;
          this.editProfileForm.patchValue(updatedUser);
          Swal.fire('Success', 'Profile updated successfully!', 'success');
          this.isSubmitting = false;
          window.location.reload();
        },
        error: err => {
          console.error('Error updating profile:', err);
          Swal.fire('Error', 'Failed to update profile.', 'error');
          this.isSubmitting = false;
        }
      });
    }, 2000);
  }





}
