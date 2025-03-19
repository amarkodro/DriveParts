import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-hero-section',
  templateUrl: './hero-section.component.html',
  styleUrls: ['./hero-section.component.css']
})
export class HeroSectionComponent implements OnInit {
  images: string[] = [
    '/assets/brembo_banner.png',
    '/assets/brembo.gif',
    '/assets/continental.gif',
    '/assets/banner_continental.png',
    '/assets/valeo_banner.png',
  ];

  currentIndex: number = 0;
  fadeState: boolean = true;
  imageChangeTimeout: any;

  ngOnInit(): void {
    this.preloadImages();
    this.scheduleNextImage();
  }

  /** Preload images to prevent blank screens */
  preloadImages(): void {
    this.images.forEach(src => {
      const img = new Image();
      img.src = src;
    });
  }

  /** Schedules the next image transition */
  scheduleNextImage(): void {
    const currentImage = this.images[this.currentIndex];

    let duration = 10000; // Default duration

    if (currentImage.endsWith('.gif')) {
      duration = this.getGifDuration(currentImage) || 10000; // Get GIF duration if available
    }

    this.imageChangeTimeout = setTimeout(() => {
      this.fadeState = false;
      setTimeout(() => {
        this.currentIndex = (this.currentIndex + 1) % this.images.length;
        this.fadeState = true;
        this.scheduleNextImage(); // Recursively schedule next image
      }, 500);
    }, duration);
  }

  /** Estimates the duration of GIFs */
  getGifDuration(imageSrc: string): number | null {
    // Here you can use an external library like gif.js to get exact duration
    return null; // Placeholder, since TypeScript can't directly read GIF metadata
  }


  get currentImage(): string {
    return this.images[this.currentIndex];
  }


  ngOnDestroy(): void {
    clearTimeout(this.imageChangeTimeout);
  }

  prevImage() {
    clearTimeout(this.imageChangeTimeout);
    this.fadeState = false;
    setTimeout(() => {
      this.currentIndex = (this.currentIndex - 1 + this.images.length) % this.images.length;
      this.fadeState = true;
      this.scheduleNextImage();
    }, 500);
  }

  nextImage() {
    clearTimeout(this.imageChangeTimeout);
    this.fadeState = false;
    setTimeout(() => {
      this.currentIndex = (this.currentIndex + 1) % this.images.length;
      this.fadeState = true;
      this.scheduleNextImage();
    }, 500);
  }
}
