import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-hero-section',
  templateUrl: './hero-section.component.html',
  styleUrls: ['./hero-section.component.css']
})
export class HeroSectionComponent implements OnInit {
  images: string[] = [
    '/assets/banner.jpg',
    '/assets/banner2_valeo.jpg',
    '/assets/continental_banner2.jpg'
  ];
  currentIndex: number = 0;

  ngOnInit(): void {
    setInterval(() => {
      this.currentIndex = (this.currentIndex + 1) % this.images.length;
    }, 3000);
  }

  get currentImage(): string {
    return this.images[this.currentIndex];
  }
}
