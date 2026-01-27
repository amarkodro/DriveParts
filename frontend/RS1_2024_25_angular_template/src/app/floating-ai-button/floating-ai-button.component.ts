import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth-services/auth.service';

@Component({
  selector: 'app-floating-ai-button',
  templateUrl: './floating-ai-button.component.html',
  styleUrls: ['./floating-ai-button.component.css']
})
export class FloatingAiButtonComponent implements OnInit {
  isLoggedIn: boolean = false;

  constructor(private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.authService.loginStatus$.subscribe(status => {
      this.isLoggedIn = status;
    });
  }

  navigateToAI() {
    this.router.navigate(['/ai']);
  }
}