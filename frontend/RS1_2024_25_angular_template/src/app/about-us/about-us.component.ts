import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-about-us',
  templateUrl: './about-us.component.html',
  styleUrl: './about-us.component.css'
})
export class AboutUsComponent implements OnInit {


   constructor(private router:Router) {
   }

    ngOnInit(): void {
        throw new Error('Method not implemented.');
    }

  NavigateToParts() {
    this.router.navigate(["/parts"]);
  }

  NavigateToFAQ() {
    this.router.navigate(['/faq'])
  }
}
