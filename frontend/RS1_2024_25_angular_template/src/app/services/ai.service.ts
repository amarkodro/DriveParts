import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatService {
  constructor(private http: HttpClient) {}
  
  askQuestion(question: string): Observable<{ answer: string, aiStatus: string }> {
    return this.http.post<{ answer: string, aiStatus: string }>(
      'http://localhost:7000/api/Chat/ask', 
      { question }
    );
  }
}
