import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import {CitiesService} from '../services/cities.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  currentStep: number = 1;
  cities: any[] = []; // Gradovi će se učitati iz baze

  constructor(private fb: FormBuilder, private http: HttpClient, private cityService:CitiesService) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      address: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      cityId: ['', Validators.required]
    });

  }

  ngOnInit(): void {
    this.loadCities(); // Poziva se kada se komponenta učita
  }

  // Učitavanje gradova iz API-ja
  loadCities() {
   this.cityService.getCity().subscribe({
     next: (data) => (this.cities = data),
   })
  }

  nextStep() {
    if (this.currentStep < 4) {
      this.currentStep++;
    }
  }

  prevStep() {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  getCityName(cityId: any): string {
    console.log('City ID received:', cityId);
    const city = this.cities.find(c => c.id == cityId);
    console.log('Matched city:', city);
    return city ? city.name : 'Unknown city';
  }


  onSubmit() {
    if (this.registerForm.valid) {
      console.log('Form Submitted', this.registerForm.value);
      // Pošalji podatke na backend
      this.http.post('http://localhost:5000/api/register', this.registerForm.value).subscribe({
        next: (response) => {
          console.log('Registration successful', response);
        },
        error: (error) => {
          console.error('Registration failed', error);
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }
}
