import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth-services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'RS1 - 2024-25 - template 1 ';

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    // Token restoration is now handled by MyAuthService constructor (runs before routing)
    // Here we just set login status and fetch user profile
    if (this.authService.isLoggedIn()) {
      this.authService.setLoginStatus(true);

      // Fetch user profile for full info
      this.authService.getUserProfile().subscribe({
        next: (user) => this.authService.setUserInfo(user),
        error: (err) => console.error('Profile fetch failed:', err)
      });
    } else {
      this.authService.setLoginStatus(false);
    }
  }
}
