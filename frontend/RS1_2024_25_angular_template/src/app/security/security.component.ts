import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthService} from '../services/auth-services/auth.service';
import {ToastrService} from 'ngx-toastr';
import firebase from 'firebase/compat/app';
import 'firebase/compat/auth';
import { environment } from '../../environments/environment';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import {Router} from '@angular/router';


declare global {
  interface Window {
    recaptchaVerifier: any;
    confirmationResult: any;
  }
}

@Component({
  selector: 'app-security',
  templateUrl: './security.component.html',
  styleUrl: './security.component.css'
})
export class SecurityComponent implements OnInit {
  securityForm!: FormGroup;
  isSubmitting: any;
  passwordStrength: 'weak' | 'medium' | 'strong' | null = null;
  showNewPassword: boolean = false;
  showConfirmPassword: boolean = false;
  showCurrentPassword: boolean = false;
  showPhoneVerificationOverlay: boolean = false;
  smsCode: string = '';
  smsCodeError: string = '';
  overlayLoading: boolean = false;
  codeTimerSeconds: number = 300;
  formattedTime: string = '05:00';
  codeTimeout: any;
  countdownInterval: any;
  showDeactivateOverlay: boolean = false;
  overlayDeactivateLoading: boolean = false;




  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router,
  ) {}


  ngOnInit(): void {
    this.securityForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern(/^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$/)
      ]],
      confirmNewPassword: ['', Validators.required],
      smsNumber: [
        '',
        [
          Validators.pattern(/^\+387\s(60|61)\s\d{3}\s\d{3,4}$/)
        ]
      ]

    }, { validators: this.passwordMatchValidator });


    this.securityForm.get('newPassword')?.valueChanges.subscribe(() => {
      this.securityForm.updateValueAndValidity({ onlySelf: true });
    });
    this.securityForm.get('confirmNewPassword')?.valueChanges.subscribe(() => {
      this.securityForm.updateValueAndValidity({ onlySelf: true });
    });

    this.securityForm.get('smsNumber')?.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.checkPhoneNumberExists();
      });

  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('newPassword')?.value;
    const confirm = form.get('confirmNewPassword')?.value;
    return password === confirm ? null : { mismatch: true };
  }

  evaluatePasswordStrength(password: string): void {
    const hasUpperCase = /[A-Z]/.test(password);
    const hasNumber = /\d/.test(password);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);

    if (password.length >= 8 && hasUpperCase && hasNumber && hasSpecialChar) {
      this.passwordStrength = 'strong';
    } else if (password.length >= 6 && (hasUpperCase || hasNumber || hasSpecialChar)) {
      this.passwordStrength = 'medium';
    } else {
      this.passwordStrength = 'weak';
    }
  }

  onSubmit(): void {
    this.securityForm.markAllAsTouched();
    if (this.securityForm.invalid) return;

    this.isSubmitting = true;
    const { currentPassword, newPassword } = this.securityForm.value;

    this.authService.changePassword(currentPassword, newPassword).subscribe({
      next: () => {
        setTimeout(() => {
          this.toastr.success('Password changed successfully');
          this.securityForm.reset();
          this.passwordStrength = null;
          this.isSubmitting = false;
        }, 2000);
      },
      error: (err) => {
        this.isSubmitting = false;
        if (err.status === 401) {
          this.securityForm.get('currentPassword')?.setErrors({ invalid: true });
          this.toastr.error('Current password is incorrect');
        } else {
          this.toastr.error('An unexpected error occurred');
        }
      }
    });
  }

  initFirebaseRecaptcha(): void {
    if (!firebase.apps.length) {
      firebase.initializeApp(environment.firebase);
    }

    if (!window.recaptchaVerifier) {
      window.recaptchaVerifier = new firebase.auth.RecaptchaVerifier(
        'recaptcha-container',
        {
          size: 'invisible',
          callback: () => {
            this.sendSMS();
          },
          'expired-callback': () => {
            console.warn('reCAPTCHA expired');
          }
        }
      );
    }
  }

  sendSMS(): void {
    const phoneNumber = this.securityForm.get('smsNumber')?.value;
    const appVerifier = window.recaptchaVerifier;

    firebase.auth().signInWithPhoneNumber(phoneNumber, appVerifier)
      .then((confirmationResult) => {
        window.confirmationResult = confirmationResult;
        this.showPhoneVerificationOverlay = true;
        this.startCodeTimer()
        this.smsCode = '';
        this.smsCodeError = '';
      })
      .catch((error) => {
        this.securityForm.get('smsNumber')?.setErrors({ invalid: true });
        this.smsCodeError = 'Failed to send code. Please check your number.';


      });
  }

  prepareRecaptchaAndSend(): void {
    this.securityForm.get('smsNumber')?.markAsTouched();

    if (this.securityForm.get('smsNumber')?.invalid) {
      return;
    }

    this.initFirebaseRecaptcha();

    setTimeout(() => {
      const appVerifier = window.recaptchaVerifier;
      if (!appVerifier) {
        return;
      }

      this.sendSMS();
    }, 500);
  }


  verifyPhoneCode(): void {
    if (!this.smsCode || this.smsCode.length < 6) {
      this.smsCodeError = 'Please enter a valid 6-digit code.';
      return;
    }

    this.overlayLoading = true;

    window.confirmationResult.confirm(this.smsCode)
      .then((result: any) => {
        clearTimeout(this.codeTimeout);
        clearInterval(this.countdownInterval);
        this.toastr.success('Phone number verified!');
        this.overlayLoading = false;
        this.showPhoneVerificationOverlay = false;

        const phoneNumber = this.securityForm.get('smsNumber')?.value;
        this.authService.enableTwoFactor(phoneNumber).subscribe({
          next: () => {
            this.toastr.success('Two-factor authentication has been enabled.');
          },
          error: () => {
            this.toastr.error('Failed to enable 2FA.');
          }
        });

      })
      .catch((error: any) => {
        this.smsCodeError = 'Invalid code. Please try again.';
        this.overlayLoading = false;
      });
  }

  pad(value: number): string {
    return value < 10 ? `0${value}` : `${value}`;
  }

  startCodeTimer(): void {
    clearTimeout(this.codeTimeout);
    clearInterval(this.countdownInterval);

    this.codeTimerSeconds = 300;
    this.formattedTime = '05:00';

    this.countdownInterval = setInterval(() => {
      this.codeTimerSeconds--;

      const minutes = Math.floor(this.codeTimerSeconds / 60);
      const seconds = this.codeTimerSeconds % 60;
      this.formattedTime = `${this.pad(minutes)}:${this.pad(seconds)}`;

      if (this.codeTimerSeconds <= 0) {
        clearInterval(this.countdownInterval);
      }
    }, 1000);

    this.codeTimeout = setTimeout(() => {
      this.showPhoneVerificationOverlay = false;
      this.smsCodeError = 'Verification time expired. Please try again.';
    }, 300000);
  }

  checkPhoneNumberExists(): void {
    const phoneNumber = this.securityForm.get('smsNumber')?.value;
    const userId = this.authService.getUserId(); // već postoji

    this.authService.checkPhone(phoneNumber, userId)
      .subscribe((res: any) => {
        if (res.exists) {
          this.securityForm.get('smsNumber')?.setErrors({ phoneTaken: true });
        } else {
          const control = this.securityForm.get('smsNumber');
          if (control?.hasError('phoneTaken')) {
            control.setErrors(null);
          }
        }
      });

  }

  confirmDeactivation(): void {
    this.overlayLoading = true;
    const username = this.authService.getUserInfoFromToken()?.username;

    this.authService.deactivate(username).subscribe({
      next: (res) => {
        this.toastr.success('Deactivation successfully.');
        this.overlayLoading = false;

        localStorage.removeItem('jwtToken');
        sessionStorage.removeItem('jwtToken');

        this.authService.setLoginStatus(false);
        this.authService.setUserInfo(null);

        this.router.navigate(['/login']);
      },
      error: (err: any) => {
        this.toastr.error(err.error , 'Deactivation failed.');
        this.overlayLoading = false;
      }
    })
  }



}







