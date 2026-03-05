import { Component, ElementRef, AfterViewInit, ViewChild, Renderer2 } from '@angular/core';
import { ViewportScroller } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth-services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { MyAuthService } from '../services/auth-services/my-auth.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements AfterViewInit {
openDashboard() {
if(this.myAuth.isAdmin()){
this.router.navigate(['/dashboard']);
}else{this.toastr.warning("Access denied");}

}

  @ViewChild('footer', { static: false }) footer!: ElementRef;

  constructor(private renderer: Renderer2, private viewportScroller: ViewportScroller, private router: Router,private authService:AuthService,private toastr:ToastrService, private myAuth:MyAuthService) {}

  ngAfterViewInit() {
    if (this.footer) {
      const observer = new IntersectionObserver(
        (entries) => {
          entries.forEach((entry) => {
            if (entry.isIntersecting) {
              this.footer.nativeElement.classList.add("show");
            }
          });
        },
        { threshold: 0.2 }
      );

      observer.observe(this.footer.nativeElement);
    }
  }

  scrollToTop(): void {
    this.viewportScroller.scrollToPosition([0, 0]);
  }

  scrollToSection(fragment: string): void {
    this.router.navigate([], { fragment: fragment }).then(() => {
      const element = document.getElementById(fragment);
      if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'start' });
      } else {
      }
    });
  }
}