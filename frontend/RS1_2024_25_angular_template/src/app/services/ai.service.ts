import {MyConfig} from '../my-config';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatService {
  constructor(private http: HttpClient) {}
  
  askQuestion(question: string): Observable<{ answer: string, aiStatus: string }> {
    return this.http.post<{ answer: string, aiStatus: string }>(
      MyConfig.api_address + '/api/Chat/ask', 
      { question }
    );
  }
}
