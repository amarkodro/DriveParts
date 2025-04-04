import {Component, OnDestroy, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {AuthService} from '../services/auth-services/auth.service';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {SocialAuthService, GoogleLoginProvider, SocialUser} from '@abacritt/angularx-social-login';

declare const google: any;
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent implements OnInit  {
  loginForm: FormGroup;
  errorMessage= '';
  showPassword: boolean = false;
  isLoading: boolean = false;
  socialUser! : SocialUser;
  tokenClient: any;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private toastr : ToastrService, private socialAuthService: SocialAuthService) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(5)]],
      rememberMe: [false]
    });
  }

  ngOnInit(): void {
    google.accounts.id.initialize({
      client_id: '609510374900-mp3inq7o0rbrcvfrg8pdivgnktkqic4r.apps.googleusercontent.com',
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
        this.authService.saveToken(token, true);
        this.authService.setLoginStatus(true);

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
        console.error("Error Google login:", err);
        this.toastr.error("Google login not successful");
      }
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;
      const credentials = this.loginForm.value;
      const rememberMe = credentials.rememberMe;

      setTimeout(() => this.isLoading = false, 1000);

      this.authService.loginUser(credentials).subscribe({
        next: (res: any) => {
          this.authService.saveToken(res.token, rememberMe);
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
          this.toastr.error('Incorrect username or password.', 'Login failed');
          this.errorMessage = 'Incorrect username or password.';
          console.error('Login error: ', err);
        }
      });

    } else {
      console.log('Form not valid');
      this.loginForm.markAllAsTouched();
      this.toastr.error('Login failed');
      setTimeout(() => this.isLoading = false, 1000);
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
}
