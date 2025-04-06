import { Component, OnInit } from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import {CitiesService} from '../services/cities.service';
import {AuthService} from '../services/auth-services/auth.service';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {GenderService} from '../services/gender.service';
import emailjs from '@emailjs/browser';
import {debounceTime} from 'rxjs/operators';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  currentStep: number = 1;
  cities: any[] = [];
  selectedFile: File | null = null;
  previewUrl: string | ArrayBuffer | null = null;
  passwordStrength: 'weak' | 'medium' | 'strong' | '' = '';
  showPassword: boolean = false;
  showConfirmPassword: boolean = false;
  genders: any[] = [];
  dropdownState: { [key: string]: boolean } = {
    gender: false,
    city: false,
  };
  selectedGenderId: number | null = null;
  selectedCityId:number | null = null;
  generatedCode: string = '';
  showVerificationOverlay: boolean = false;
  userEnteredCode: string = '';
  enteredResetCode: any;
  codeError: any;
  codeAttemptCount: number = 0;
  codeTimer: any;
  timeLeft: number = 300;
  timerExpired: boolean = false;
  overlayLoading: boolean = false;
  isNextLoading: boolean = false;
  isSubmitLoading: boolean = false;
  lastCheckedPhone: string | null = null;

  constructor(private fb: FormBuilder,
              private http: HttpClient,
              private cityService:CitiesService,
              private authService: AuthService,
              private router: Router,
              private toastr: ToastrService,
              private genderService: GenderService,
              ) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      genderId: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required,
        Validators.pattern(/^\+387 6\d \d{3} \d{3,4}$/)
      ]],
      address: ['', Validators.required],
      profileImage: [null],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6), this.passwordStrengthValidator]],
      confirmPassword: ['', Validators.required],
      cityId: ['', Validators.required]
    });

    this.registerForm.setValidators(this.passwordMatchValidator);


  }

  ngOnInit(): void {
    this.loadCities();
    this.loadGenders();
    this.registerForm.get('password')?.valueChanges.subscribe(value => {
      this.evaluatePasswordStrength(value);
    });

    this.registerForm.get('phoneNumber')?.valueChanges
      .pipe(debounceTime(800)) // 800ms nakon što korisnik prestane kucati
      .subscribe(() => {
        this.checkPhone();
      });
  }

  loadCities() {
   this.cityService.getCity().subscribe({
     next: (data) => (this.cities = data),
   });

    const existingCityId = this.registerForm.get('cityId')?.value;
    if (existingCityId) {
      this.selectedCityId = existingCityId;
    }
  }

  nextStep() {
    if (this.isNextLoading) return; // spriječi dupli klik
    this.isNextLoading = true;

    setTimeout(() => {
      if (this.currentStep === 1) {
        this.registerForm.get('name')?.markAsTouched();
        this.registerForm.get('surname')?.markAsTouched();
        this.registerForm.get('genderId')?.markAsTouched();
        if (
          this.registerForm.get('name')?.invalid ||
          this.registerForm.get('surname')?.invalid ||
          this.registerForm.get('genderId')?.invalid
        ) {
          this.isNextLoading = false;
          return;
        }
      }

      if (this.currentStep === 2) {
        this.registerForm.get('email')?.markAsTouched();
        this.registerForm.get('phoneNumber')?.markAsTouched();
        this.registerForm.get('address')?.markAsTouched();
        this.registerForm.get('cityId')?.markAsTouched();

        if (
          this.registerForm.get('email')?.invalid ||
          this.registerForm.get('phoneNumber')?.invalid ||
          this.registerForm.get('address')?.invalid ||
          this.registerForm.get('cityId')?.invalid
        ) {
          this.isNextLoading = false;
          return;
        }

        const email = this.registerForm.get('email')?.value;
        this.sendVerificationCode(email);
        this.isNextLoading = false;
        return;
      }

      if (this.currentStep === 3) {
        this.registerForm.get('username')?.markAsTouched();
        this.registerForm.get('password')?.markAsTouched();
        this.registerForm.get('confirmPassword')?.markAsTouched();

        const password = this.registerForm.get('password')?.value;
        const confirmPassword = this.registerForm.get('confirmPassword')?.value;

        if (
          this.registerForm.get('username')?.invalid ||
          this.registerForm.get('password')?.invalid ||
          this.registerForm.get('confirmPassword')?.invalid ||
          password !== confirmPassword
        ) {
          if (password !== confirmPassword) {
            this.toastr.error('Passwords do not match', 'Validation Error');
          }
          this.isNextLoading = false;
          return;
        }
      }

      this.currentStep++;
      this.isNextLoading = false;
    }, 2000);
  }


  prevStep() {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  getCityName(cityId: any): string {
    const city = this.cities.find(c => c.id == cityId);
    return city ? city.name : 'Unknown city';
  }

  onSubmit() {
    if (!this.registerForm.valid) {
      console.log('Form is invalid');
      this.toastr.error("Please fill out the form correctly.");
      return;
    }

    this.isSubmitLoading = true;

    if (this.registerForm.value.cityId) {
      this.registerForm.patchValue({ cityId: parseInt(this.registerForm.value.cityId, 10) });
    }

    const formData = new FormData();

    for (const key in this.registerForm.value) {
      if (key !== 'profileImage') {
        formData.append(key, this.registerForm.value[key]);
      }
    }

    if (this.selectedFile) {
      formData.append('profileImage', this.selectedFile, this.selectedFile.name);
    }

    this.authService.registerUser(formData).subscribe({
      next: () => {
        this.toastr.success("Registration successful!");
        this.router.navigate(['/login']);
        this.isSubmitLoading = false;
      },
      error: (error) => {
        console.error("Registration error:", error);
        this.toastr.error("Registration failed");
        this.isSubmitLoading = false;
      }
    });
  }



  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.registerForm.patchValue({ profileImage: file });

      const reader = new FileReader();
      reader.onload = () => {
        this.previewUrl = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  passwordMatchValidator: ValidatorFn = (group: AbstractControl): ValidationErrors | null => {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  };

  checkUsername() {
    const username = this.registerForm.get('username')?.value;
    if (!username) return;

    this.authService.checkUsername(username).subscribe((res: any) => {
      if (res.exists) {
        this.registerForm.get('username')?.setErrors({ exists: true });
        this.toastr.error('Username already exists');
      }
    });
  }

  checkEmail() {
    const email = this.registerForm.get('email')?.value;
    if (!email) return;

    this.authService.checkEmail(email).subscribe((res: any) => {
      if (res.exists) {
        this.registerForm.get('email')?.setErrors({ exists: true });
        this.toastr.error('Email already exists');
      }
    });
  }

  checkPhone() {
    const phone = this.registerForm.get('phoneNumber')?.value;
    console.log("Checking phone number:", phone);
    if (!phone || phone === this.lastCheckedPhone) return;

    this.lastCheckedPhone = phone;

    this.authService.checkPhone(phone).subscribe((res: any) => {
      if (res.exists) {
        this.registerForm.get('phoneNumber')?.setErrors({ exists: true });
        this.registerForm.get('phoneNumber')?.markAsTouched();
        this.toastr.error('Phone number already exists');
      } else {
        if (this.registerForm.get('phoneNumber')?.hasError('exists')) {
          this.registerForm.get('phoneNumber')?.setErrors(null);
        }
      }
    });
  }

  passwordStrengthValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) return null;

    const hasUpperCase = /[A-Z]/.test(value);
    const hasNumber = /[0-9]/.test(value);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);

    const valid = hasUpperCase && hasNumber && hasSpecialChar;
    return valid ? null : { weakPassword: true };
  };


  evaluatePasswordStrength(password: string) {
    const hasUpperCase = /[A-Z]/.test(password);
    const hasNumber = /[0-9]/.test(password);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);
    const isLongEnough = password.length >= 6;

    if (hasUpperCase && hasNumber && hasSpecialChar && isLongEnough) {
      this.passwordStrength = 'strong';
    } else if (
      (hasUpperCase && hasNumber) ||
      (hasUpperCase && hasSpecialChar) ||
      (hasNumber && hasSpecialChar)
    ) {
      this.passwordStrength = 'medium';
    } else if (password.length > 0) {
      this.passwordStrength = 'weak';
    } else {
      this.passwordStrength = '';
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  loadGenders() {
    this.genderService.getGenders().subscribe({
      next: (data) => {
        this.genders = data;
        console.log('Genders loaded:', this.genders);

        const existingGenderId = this.registerForm.get('genderId')?.value;
        if (existingGenderId) {
          this.selectedGenderId = existingGenderId;
        }
      },
      error: (error) => {
        console.error('Error loading genders:', error);
        this.toastr.error('Failed to load genders');
      }
    });
  }

  toggleDropdown(dropdownName: string) {
    this.dropdownState[dropdownName] = !this.dropdownState[dropdownName];
  }

  selectGender(genderId: number) {
    this.selectedGenderId = genderId;
    this.registerForm.patchValue({ genderId });
    this.dropdownState['gender'] = false;
  }

  getSelectedGenderName() {
    const gender = this.genders.find(g => g.id === this.selectedGenderId);
    return gender ? gender.genderName : null;
  }

  getSelectedCityName() {
    const city = this.cities.find(c => c.id === this.selectedCityId);
    return city ? city.name : null;
  }

  selectCity(cityId: number) {
    this.selectedCityId = cityId;
    this.registerForm.patchValue({ cityId });
    this.dropdownState['city'] = false;
  }


  sendVerificationCode(email: string) {
    const verificationCode = Math.floor(100000 + Math.random() * 900000).toString();

    this.generatedCode = verificationCode;
    this.codeAttemptCount = 0;
    this.startCodeTimer();

    const templateParams = {
      verification_code: verificationCode,
      to_email: email
    };

    emailjs.send(
      'service_xh0d98k',
      'template_tdwpnbe',
      templateParams,
      'B8xPgvirRSkYNmw9g'
    )
      .then(() => {
        console.log('Email sent!');

        this.showVerificationOverlay = true;
      })
      .catch((error) => {
        console.error('Error sending email:', error);
      });
  }

  verifyCode() {
    if (this.timerExpired) {
      this.toastr.error("Verification code has expired!");
      return;
    }

    this.validateEnteredCode();
    if (this.codeError) {
      this.toastr.error(this.codeError);
      return;
    }

    this.overlayLoading = true;

    setTimeout(() => {
      if (this.enteredResetCode === this.generatedCode) {
        this.toastr.success("The code has been successfully verified!!");
        this.showVerificationOverlay = false;
        this.currentStep++;
        this.codeAttemptCount = 0;
      } else {
        this.codeAttemptCount++;
        this.codeError = 'Incorrect code. Try again.';
        this.toastr.error(`Incorrect code. Attempt ${this.codeAttemptCount}/3`);
        this.enteredResetCode = '';

        if (this.codeAttemptCount >= 3) {
          this.toastr.error("3 incorrect attempts. Please try again.");
          this.closeVerificationOverlay();
        }
      }

      this.overlayLoading = false;
    }, 2000);
  }



  closeVerificationOverlay() {
    this.showVerificationOverlay = false;
    this.enteredResetCode = '';
    this.codeError = '';
    this.codeAttemptCount = 0;
  }



  validateEnteredCode() {
    const codeRegex = /^\d{6}$/;
    if (!this.enteredResetCode) {
      this.codeError = 'Verification code is required.';
    } else if (!codeRegex.test(this.enteredResetCode)) {
      this.codeError = 'Code must be exactly 6 digits.';
    } else {
      this.codeError = '';
    }
  }

  cancelVerification() {
    this.showVerificationOverlay = false;
    this.userEnteredCode = '';
  }

  startCodeTimer() {
    this.timeLeft = 300;
    this.timerExpired = false;

    this.codeTimer = setInterval(() => {
      if (this.timeLeft > 0) {
        this.timeLeft--;
      } else {
        this.timerExpired = true;
        clearInterval(this.codeTimer);
        this.closeVerificationOverlay();
        this.toastr.error("Verification code expired. Please try again.");
      }
    }, 1000);
  }

  get formattedTimeLeft(): string {
    const minutes = Math.floor(this.timeLeft / 60);
    const seconds = this.timeLeft % 60;
    return `${minutes}:${seconds < 10 ? '0' + seconds : seconds}`;
  }








}
