import {Component, OnDestroy, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {AuthService} from '../services/auth-services/auth.service';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';

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

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private toastr : ToastrService) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(5)]],
      rememberMe: [false]
    });
  }

  ngOnInit(): void {

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

          // 🔥 GET USER PROFILE nakon logina
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
