import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PartService, Part, CategoryResponse, ManufacturerResponse } from '../services/Adminpart.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { empty, of } from 'rxjs';
import { forkJoin } from 'rxjs';
@Component({
  selector: 'app-admin-part-form',

  templateUrl: './admin-part-form.component.html',
  styleUrls: ['./admin-part-form.component.css']
})
export class AdminPartFormComponent implements OnInit {
  partForm: FormGroup;
  isEditMode: boolean = false;
  partId: number | null = null;
  selectedFile: File | null = null;
  previewUrl: string | ArrayBuffer | null = null;
  part:Part={
    partId: 0,
    name: '',
    price: 0,  
    categoryId: 0,
  manufacturerId: 0,
    partImage:'',
    description: '',
    isFeatured: false,
    isOnSale: false,
    isNewArrival: false,
    type: ''
  }
  categories: any[] = [];
  manufacturers: any[] = [];
  constructor(
    private partService: PartService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.partForm = this.fb.group({
      Name: ['', Validators.required],
      Price: [null, [Validators.required, Validators.min(0.01)]],
      CategoryId: [null, Validators.required],
      ManufacturerId: [null, Validators.required],
      PartImage: [''],
      Description: ['', Validators.required],
      IsFeatured: [false],
      IsOnSale: [false],
      IsNewArrival: [false]
    });
  }

ngOnInit(): void {
  this.partId = this.route.snapshot.params['id'];
  console.log('AdminPartFormComponent initialized');

  // Load categories and manufacturers first
  forkJoin({
    categories: this.partService.getCategories(),
    manufacturers: this.partService.getManufacturers(),
    part: this.partId ? this.partService.getPart(this.partId) : of(null)
  }).subscribe({
    next: ({ categories, manufacturers, part: fetchedPart }) => { // Rename to avoid conflict
    this.categories = categories;
    this.manufacturers = manufacturers;

   if (this.partId) {
        this.partService.getPart(this.partId).subscribe(part => {
          this.isEditMode = true;
          this.initializeFormWithPart(part);
        });
    }
    },
    error: (err) => console.error('Error loading dropdowns:', err)
  });
}
private initializeFormWithPart(part: Part): void {


  this.partForm.patchValue({
 Name: part.name,
  Price: part.price,
  CategoryId: part.categoryId,      // Direct ID assignment
  ManufacturerId: part.manufacturerId,
  Description: part.description,
  IsFeatured: part.isFeatured,
  IsOnSale: part.isOnSale,
   PartImage: part.partImage,
  IsNewArrival: part.isNewArrival
  });

console.log("Existing PartImage path:", part.partImage);
}

getImageUrl(): string {
  if (this.partForm.value.PartImage) {
    return `http://localhost:7000${this.partForm.value.PartImage}`;
  }
  return 'https://via.placeholder.com/300x200';
}
  loadPart(): void {
    
  if (this.partId) {
    
    this.partService.getPart(this.partId).subscribe(part => {
       console.log('Loaded part:', part);

      this.partForm.patchValue({
        Name: part.name,
        Price: part.price,
        CategoryId: part.categoryId,       // Direct ID assignment
        ManufacturerId: part.manufacturerId, // Direct ID assignment
        Description: part.description,
        IsFeatured: part.isFeatured,
        IsOnSale: part.isOnSale,
        IsNewArrival: part.isNewArrival
      });
const category = this.categories.find(c => c.categoryId === part.categoryId);
      const manufacturer = this.manufacturers.find(m => m.manufacturerId === part.manufacturerId);
      console.log('Category:', category?.name);
      console.log('Manufacturer:', manufacturer?.name);
    });

  }
}


onSubmit(): void {
  if (this.partForm.invalid) {
    console.error('Form is invalid', this.partForm.errors);
    return;
  }
const formValue = this.partForm.value;
  const formData = new FormData();
  formData.append('Name', this.partForm.value.Name);
  formData.append('Price', this.partForm.value.Price.toString());
  formData.append('Description', this.partForm.value.Description);
 formData.append('CategoryId', this.partForm.value.CategoryId?.toString() ?? '');
formData.append('ManufacturerId', this.partForm.value.ManufacturerId?.toString() ?? '');
formData.append('IsFeatured', this.partForm.value.IsFeatured?.toString() ?? 'false');
formData.append('IsOnSale', this.partForm.value.IsOnSale?.toString() ?? 'false');
formData.append('IsNewArrival', this.partForm.value.IsNewArrival?.toString() ?? 'false');

formData.append('ExistingImagePath', this.partForm.value.partImage);

if (this.selectedFile) {
  formData.append('PartImage', this.selectedFile);
}
if (!this.partForm.value.CategoryId || !this.partForm.value.ManufacturerId) {
  alert('Please select both a Category and Manufacturer.');
  return;
}
  if (!this.selectedFile && this.partForm.value.PartImage) {
  formData.append('ExistingImagePath', this.partForm.value.PartImage);
}
  if (this.isEditMode && this.partId !== null) {
    this.partService.updatePartFormData(this.partId, formData).subscribe({
      next: () => this.router.navigate(['/edit']),
      error: (err) => console.error('Update error:', err)
    });
  } else {
    this.partService.addPart(formData).subscribe({
      next: (response) => {
        console.log('Part added successfully', response);
        this.router.navigate(['/edit']);
      },
      error: (error) => {
        console.error('Failed to add part:', error);
      }
    });
  }
}
private markFormGroupTouched(formGroup: FormGroup) {
  Object.values(formGroup.controls).forEach(control => {
    control.markAsTouched();
    if (control instanceof FormGroup) {
      this.markFormGroupTouched(control);
    }
  });
}

  cancel(): void {
    this.router.navigate(['/edit']); // Cancel editing/adding and go back to parts list
  }
onFileSelected(event: Event): void {
  const input = event.target as HTMLInputElement;
  if (input.files?.length) {
    this.selectedFile = input.files[0];

    // Generate preview
    const reader = new FileReader();
    reader.onload = () => {
      this.previewUrl = reader.result;
    };
    reader.readAsDataURL(this.selectedFile);

    // Optional: update PartImage with filename
    this.partForm.patchValue({ PartImage: this.selectedFile.name });
  }
}
  
   
}
