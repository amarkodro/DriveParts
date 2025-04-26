import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PromoCodeService {

  private apiUrl = 'http://localhost:7000/api/PromoCode';

  constructor(private http: HttpClient) { }

  checkCode(code: string) {
    return this.http.get<{ id: number, discount: number}>(`${this.apiUrl}/check/${code}`);
  }
}
