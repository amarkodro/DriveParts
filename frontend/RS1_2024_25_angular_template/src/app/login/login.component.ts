import {Component, OnDestroy, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormsModule } from '@angular/forms';
import {AuthService} from '../services/auth-services/auth.service';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {SocialAuthService, GoogleLoginProvider, SocialUser} from '@abacritt/angularx-social-login';
import emailjs from '@emailjs/browser';


declare const google: any;
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})


export class LoginComponent implements OnInit  {
  loginForm: FormGroup;
  errorMessage= '';
  showPassword: boolean = false;
  isLoading: boolean = false;
  socialUser! : SocialUser;
  tokenClient: any;
  showForgotPasswordOverlay = false;
  showCodeVerificationOverlay = false;
  forgotPasswordEmail = '';
  enteredResetCode = '';
  generatedResetCode = '';
  emailError: string = '';
  codeError : string = '';
  showResetPasswordOverlay = false;
  newPassword = '';
  confirmNewPassword = '';
  passwordMismatchError = '';
  overlayLoading: boolean = false;
  isClosingOverlay: boolean = false;
  showNewPasswordOverlay: boolean = false;
  showConfirmNewPasswordOverlay: boolean = false;
  timeLeft: number = 300;
  codeTimer: any;
  timerExpired: boolean = false;
  codeAttemptCount: number = 0;
  showReactivateOverlay: boolean = false;
  showReactivateCodeOverlay: boolean = false;

  reactivateEmail: string = '';
  reactivationCode: string = '';
  enteredReactivationCode: string = '';
  reactivationError: string = '';
  reactivationLoading: boolean = false;



  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private toastr : ToastrService, private socialAuthService: SocialAuthService) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(5)]],
      rememberMe: [false]
    });
  }

  ngOnInit(): void {
    google.accounts.id.initialize({
      client_id: '875789338933-01mi71kk9dinvbc1lap0nila0u5m4q01.apps.googleusercontent.com',
      callback: (response: any) => this.handleCredentialResponse(response)
    });

    google.accounts.id.renderButton(
      document.getElementById('hidden-google-btn'),
      { theme: 'outline', size: 'large', type: 'standard' }
    );
  }

  triggerGoogleLogin(): void {
    const googleBtn = document.querySelector('#hidden-google-btn div[role=button]') as HTMLElement;
    if (googleBtn) googleBtn.click();
    else console.error("Google button not found");
  }

  handleCredentialResponse(response: any): void {
    const idToken = response.credential;

    this.authService.googleLogin(idToken).subscribe({
      next: (res: any) => {
        const token = res.token;
        const refreshToken = res.refreshToken;

        this.authService.saveToken(token, true);
        if (refreshToken) {
          this.authService.saveRefreshToken(refreshToken);
        }


        this.authService.getUserProfile().subscribe({
          next: user => {
            this.authService.setUserInfo(user);
            this.router.navigate(['/']);
          },
          error: err => console.error("Error in getUserProfile:", err)
        });

        this.toastr.success("Login with Google successful!");
      },
      error: err => {
        const backendMsg = err?.error;

        if (typeof backendMsg === 'string' && backendMsg.includes('deactivated')) {
          this.errorMessage = backendMsg;
          this.reactivateEmail = this.extractEmailFromIdToken(idToken);  // ✅ automatski uzimamo email
          this.showReactivateOverlay = true;
          this.toastr.warning("Your account is deactivated. You can reactivate below.");
        } else {
          this.toastr.error("Google login not successful");
        }

        console.error("Error Google login:", err);
      }
    });
  }

  extractEmailFromIdToken(idToken: string): string {
    try {
      const payload = JSON.parse(atob(idToken.split('.')[1]));
      return payload.email || '';
    } catch (e) {
      console.error("Failed to extract email from ID token:", e);
      return '';
    }
  }



  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;
      const credentials = this.loginForm.value;
      const rememberMe = credentials.rememberMe;

      setTimeout(() => this.isLoading = false, 1000);

      this.authService.loginUser(credentials).subscribe({
        next: (res: any) => {
          const token = res?.token;

          if (!token || typeof token !== 'string') {
            this.toastr.error('Login failed: Invalid token received');
            return;
          }


          this.authService.saveToken(token, rememberMe);
          if(res.refreshToken) {
            this.authService.saveRefreshToken(res.refreshToken);
          }
          this.authService.setLoginStatus(true);

          this.toastr.success(`Welcome back, ${credentials.username}!`, `Login successful`);


          this.authService.getUserProfile().subscribe({
            next: (user) => {
              this.authService.setUserInfo(user);
              this.router.navigate(['/']);
            },
            error: () => {
              this.router.navigate(['/']);
            }
          });
        },
        error: (err: any) => {
          const backendMsg = err?.error;

          if (typeof backendMsg === 'string' && backendMsg.includes('deactivated')) {
            this.toastr.warning(backendMsg);
            this.errorMessage = backendMsg;
          } else {
            this.toastr.error('Incorrect username or password.', 'Login failed');
            this.errorMessage = 'Incorrect username or password.';
          }

          this.isLoading = false;
          console.error('Login error: ', err);
        }
      });

    } else {
      this.loginForm.markAllAsTouched();
      this.toastr.error('Login failed');
      setTimeout(() => this.isLoading = false, 1000);
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  openForgotPasswordOVerlay($event: MouseEvent) {
    $event.preventDefault();
    this.showForgotPasswordOverlay = true;
  }

  cancelForgotPassword() {
     this.showForgotPasswordOverlay = false;
     this.forgotPasswordEmail = '';
  }

  sendResetCode() {
    this.validateEmail();
    if (this.emailError) return;

    if (!this.forgotPasswordEmail) {
      this.toastr.error("Please enter your email.");
      return;
    }

    this.overlayLoading = true;

    this.authService.checkEmail(this.forgotPasswordEmail).subscribe({
      next: (res: any) => {
        if (res.exists) {
          this.generatedResetCode = Math.floor(100000 + Math.random() * 900000).toString();
           this.codeAttemptCount = 0;
          emailjs.send(
            'service_xh0d98k',
            'template_hishzyg',
            {
              verification_code: this.generatedResetCode,
              to_email: this.forgotPasswordEmail,
            },
            'B8xPgvirRSkYNmw9g'
          ).then(() => {
            setTimeout(() => {
              this.toastr.success('Verification code sent!');
              this.overlayLoading = false;
              this.showForgotPasswordOverlay = false;
              this.showCodeVerificationOverlay = true;
              this.startCodeTimer()
            }, 2000);
          }).catch((error) => {
            setTimeout(() => {
              this.toastr.error('Failed to send email');
              console.error(error);
              this.overlayLoading = false;
            }, 2000);
          });

        } else {
          setTimeout(() => {
            this.emailError = "This email is not registered.";
            this.forgotPasswordEmail += " ";
            this.forgotPasswordEmail = this.forgotPasswordEmail.trim();
            this.toastr.error("This email is not registered.");
            this.overlayLoading = false;
          }, 2000);
        }
      },
      error: (err) => {
        setTimeout(() => {
          this.toastr.error("Error while checking email.");
          console.error(err);
          this.overlayLoading = false;
        }, 2000);
      }
    });
  }

  verifyResetCode() {
    if (this.timerExpired) {
      this.toastr.error("Verification code has expired!");
      return;
    }

    this.validateCode();
    if (this.codeError) {
      this.toastr.error(this.codeError);
      return;
    }

    this.overlayLoading = true;

    setTimeout(() => {
      if (this.enteredResetCode === this.generatedResetCode) {
        this.toastr.success('Code verified! Proceed to reset password.');
        this.showCodeVerificationOverlay = false;
        this.showResetPasswordOverlay = true;
        this.codeAttemptCount = 0;
      } else {
        this.codeAttemptCount++;
        this.codeError = 'Incorrect code. Try again.';
        this.toastr.error(`Incorrect code. Try again. ${this.codeAttemptCount}/3`);
        this.enteredResetCode = '';
        if (this.codeAttemptCount >= 3) {
          this.toastr.error("You entered the wrong code 3 times. Please try again.");
          this.codeAttemptCount = 0;
          this.enteredResetCode = '';
          this.closeOverlay('code');
          this.showForgotPasswordOverlay = true;
        }
      }

      this.overlayLoading = false;
    }, 2000);
  }


  validateEmail() {
    if (!this.forgotPasswordEmail) {
      this.emailError = 'Email is required.';
    } else if (!/\S+@\S+\.\S+/.test(this.forgotPasswordEmail)) {
      this.emailError = 'Invalid email format.';
    } else {
      this.emailError = '';
    }
  }

  validateCode() {
    const codeRegex = /^\d{6}$/; // Tačno 6 cifara
    if (!this.enteredResetCode) {
      this.codeError = 'Verification code is required.';
    } else if (!codeRegex.test(this.enteredResetCode)) {
      this.codeError = 'Code must be exactly 6 digits.';
    } else {
      this.codeError = '';
    }
  }

  submitNewPassword() {
    this.passwordMismatchError = '';

    const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$/;

    if (!this.newPassword || !this.confirmNewPassword) {
      this.passwordMismatchError = "Please fill in both fields.";
      return;
    }

    if (this.newPassword !== this.confirmNewPassword) {
      this.passwordMismatchError = "Passwords do not match.";
      return;
    }

    if (!passwordRegex.test(this.newPassword)) {
      this.passwordMismatchError = "At least 6 chars, uppercase, num & special.";
      return;
    }

    const payload = {
      email: this.forgotPasswordEmail,
      newPassword: this.newPassword
    };

    this.overlayLoading = true;

    setTimeout(() => {
      this.authService.resetPassword(payload).subscribe({
        next: () => {
          this.toastr.success("Password successfully updated.");
          this.showResetPasswordOverlay = false;
          this.newPassword = '';
          this.confirmNewPassword = '';
          this.forgotPasswordEmail = '';
          this.enteredResetCode = '';
          this.overlayLoading = false;
        },
        error: (err) => {
          console.error('Reset password error:', err);
          this.toastr.error("Failed to update password. Try again.");
          this.overlayLoading = false;
        }
      });
    }, 2000);
  }

  closeOverlay(type: 'email' | 'code' | 'reset') {
    this.isClosingOverlay = true;

    setTimeout(() => {
      this.isClosingOverlay = false;

      if (type === 'email') {
        this.showForgotPasswordOverlay = false;
      } else if (type === 'code') {
        this.showCodeVerificationOverlay = false;
      } else if (type === 'reset') {
        this.showResetPasswordOverlay = false;
      }
    }, 300);
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
        this.closeOverlay('code');
        this.showForgotPasswordOverlay = true;
        this.toastr.error("Verification code expired. Please request a new one.");
      }
    }, 1000);
  }

  get formattedTimeLeft(): string {
    const minutes = Math.floor(this.timeLeft / 60);
    const seconds = this.timeLeft % 60;
    const paddedSeconds = seconds < 10 ? '0' + seconds : seconds;
    return `${minutes}:${paddedSeconds}`;
  }

  openRestoreOverlay(event: MouseEvent) {
    event.preventDefault();
    this.showReactivateOverlay = true;
    this.reactivateEmail = '';
    this.reactivationCode = '';
    this.enteredReactivationCode = '';
    this.reactivationError = '';
  }

  sendReactivationCode() {
    if (!this.reactivateEmail || !this.reactivateEmail.includes('@')) {
      this.reactivationError = 'Please enter a valid email address.';
      return;
    }

    this.reactivationCode = Math.floor(100000 + Math.random() * 900000).toString();

    const templateParams = {
      to_email: this.reactivateEmail,
      verification_code: this.reactivationCode
    };

    this.reactivationLoading = true;

    emailjs.send('service_xh0d98k', 'template_tdwpnbe', templateParams, 'B8xPgvirRSkYNmw9g')
      .then(() => {
        this.reactivationLoading = false;
        this.showReactivateOverlay = false;
        this.showReactivateCodeOverlay = true;
      })
      .catch(err => {
        this.reactivationLoading = false;
        this.reactivationError = 'Failed to send verification code.';
        console.error('EmailJS error:', err);
      });
  }

  verifyReactivationCode() {
    const codeRegex = /^\d{6}$/;

    if (!this.enteredReactivationCode.trim()) {
      this.reactivationError = 'Please enter the code.';
      return;
    }

    if (!codeRegex.test(this.enteredReactivationCode)) {
      this.reactivationError = 'Code must be exactly 6 digits.';
      return;
    }

    this.overlayLoading = true;

    setTimeout(() => {
      if (this.enteredReactivationCode === this.reactivationCode) {
        this.authService.reactivateProfile(this.reactivateEmail).subscribe({
          next: () => {
            this.toastr.success("Your account has been reactivated. You can now log in.");
            this.showReactivateCodeOverlay = false;
            this.reactivationError = '';
            this.reactivationCode = '';
            this.enteredReactivationCode = '';
          },
          error: () => {
            this.reactivationError = "Failed to reactivate account. Try again.";
          },
          complete: () => {
            this.overlayLoading = false;
          }
        });
      } else {
        this.reactivationError = 'Incorrect code.';
        this.overlayLoading = false;
      }
    }, 2000);
  }

  onCodeInputChange() {
    if (this.reactivationError) {
      this.reactivationError = '';
    }
  }


}