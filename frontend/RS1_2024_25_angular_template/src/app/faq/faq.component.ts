import { Component, OnInit } from '@angular/core';
import { FaqService, FAQ } from '../services/faq.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-faq',
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.css']
})
export class FaqComponent implements OnInit {
  faqs: FAQ[] = [];
  faqForm!: FormGroup;
  isSending: any;

  constructor(private faqService: FaqService, private fb: FormBuilder, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.faqService.getTop10FAQs().subscribe({
      next: data => this.faqs = data,
      error: err => console.error('Failed to load FAQs', err)
    });

    this.faqForm = this.fb.group({
      question: ['', Validators.required],
    })

  }

  toggle(faq: FAQ) {
    faq.open = !faq.open;
  }

  submitQuestion() {
    if(this.faqForm.invalid) return;

    this.isSending = true;

    const payload = {
      question: this.faqForm.value.question
    };

    setTimeout(() => {
      this.faqService.addFaq(payload).subscribe({
        next: () => {
          this.faqForm.reset();
          this.isSending = false;
          this.toastr.success('Successfully added successfully');
        },
        error: () => {
          this.toastr.error('Failed to submit your question.');
          this.isSending = false;
        }
      });
    }, 2000);
  }
}
