import { Component, ElementRef, ViewChild, AfterViewChecked } from '@angular/core';
import { ChatService } from '../services/ai.service';

@Component({
  selector: 'app-chat',
  templateUrl: './ai-chat.component.html',
  styleUrls: ['./ai-chat.component.css']
})
export class ChatComponent implements AfterViewChecked {
  newQuestion = '';
  question = '';
  answer = '';
  aiStatus = 'Checking AI status...';
  isTyping = false;
  
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;

  constructor(private chatService: ChatService) {}

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.messagesContainer.nativeElement.scrollTop = 
        this.messagesContainer.nativeElement.scrollHeight;
    } catch(err) { }
  }

  ask() {
    const question = this.newQuestion.trim();
    if (!question) return;

    // Add user message to history
    this.question = question;
    this.newQuestion = '';
    this.answer = '';
    this.isTyping = true;
    
    // Get AI response
    this.chatService.askQuestion(question).subscribe({
      next: res => {
        this.isTyping = false;
        this.answer = res.answer;
        this.aiStatus = res.aiStatus;
      },
      error: err => {
        this.isTyping = false;
        this.answer = "AI service unavailable. Please try again later.";
        this.aiStatus = "Service offline";
        console.error(err);
      }
    });
  }
}