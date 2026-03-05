import {MyConfig} from '../my-config';
import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';

export interface FAQ {
  faqId: number;
  question: string;
  answer: string;
  open?: boolean;
}

@Injectable({
  providedIn: 'root'
})



export class FaqService {
  private apiUrl: string = MyConfig.api_address + '/api/FAQ';

  constructor(private http: HttpClient) { }

  getTop10FAQs() {
    return this.http.get<FAQ[]>(`${this.apiUrl}/get_10`);
  }

  addFaq(faq: any) {
    return this.http.post<FAQ[]>(`${this.apiUrl}/add`, faq);
  }
}
