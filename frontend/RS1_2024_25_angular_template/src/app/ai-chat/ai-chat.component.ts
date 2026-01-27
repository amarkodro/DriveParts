import { Component, ElementRef, ViewChild, AfterViewChecked, OnInit, NgZone } from '@angular/core';
import { ChatService } from '../services/ai.service';
import { ChatHistoryService } from '../services/chat-history.service';

@Component({
  selector: 'app-chat',
  templateUrl: './ai-chat.component.html',
  styleUrls: ['./ai-chat.component.css']
})
export class ChatComponent implements AfterViewChecked, OnInit {
  newQuestion = '';
  aiStatus = 'Checking AI status...';
  isTyping = false;
  isListening = false;
  messages: { role: string, content: string }[] = [];
  recognition: any;
  silenceTimer: any; // Timer for silence detection

  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;

  constructor(
    private chatService: ChatService,
    private chatHistoryService: ChatHistoryService,
    private ngZone: NgZone
  ) { }

  ngOnInit() {
    this.messages = this.chatHistoryService.getMessages();
    this.checkSpeechSupport();
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.messagesContainer.nativeElement.scrollTop =
        this.messagesContainer.nativeElement.scrollHeight;
    } catch (err) { }
  }

  checkSpeechSupport() {
    if ('webkitSpeechRecognition' in window || 'SpeechRecognition' in window) {
      this.setupRecognition();
    } else {
      console.warn('Speech recognition not supported in this browser');
    }
  }

  setupRecognition() {
    const SpeechRecognition = (window as any).webkitSpeechRecognition || (window as any).SpeechRecognition;
    this.recognition = new SpeechRecognition();
    this.recognition.continuous = false;
    this.recognition.interimResults = true;
    this.recognition.lang = 'en-US';

    this.recognition.onresult = (event: any) => {
      this.ngZone.run(() => { // Wrap in ngZone
        this.resetSilenceTimer();
        let transcript = '';
        for (let i = event.resultIndex; i < event.results.length; ++i) {
          if (event.results[i].isFinal) {
            transcript += event.results[i][0].transcript;
          }
        }
        this.newQuestion = transcript;
      });
    };

    this.recognition.onerror = (event: any) => {
      this.ngZone.run(() => { // Wrap in ngZone
        console.error('Speech recognition error', event.error);
        this.stopVoiceRecognition();
      });
    };

    this.recognition.onend = () => {
      this.ngZone.run(() => { // Wrap in ngZone
        this.stopVoiceRecognition();
      });
    };
  }

  // Start silence detection timer
  startSilenceTimer() {
    // Stop after 1.5 seconds of silence
    this.silenceTimer = setTimeout(() => {
      if (this.isListening) {
        this.recognition.stop();
      }
    }, 5000);
  }

  // Reset the silence timer
  resetSilenceTimer() {
    if (this.silenceTimer) {
      clearTimeout(this.silenceTimer);
    }
    this.startSilenceTimer();
  }

  // Clean up silence timer
  clearSilenceTimer() {
    if (this.silenceTimer) {
      clearTimeout(this.silenceTimer);
      this.silenceTimer = null;
    }
  }

  toggleVoiceRecognition() {
    if (!this.recognition) {
      alert('Speech recognition not supported in this browser. Try Chrome or Edge.');
      return;
    }

    if (this.isListening) {
      this.recognition.stop(); // Properly stop recognition
    } else {
      this.startVoiceRecognition();
    }
  }
  startVoiceRecognition() {
    this.isListening = true;
    this.newQuestion = '';
    this.recognition.start();
    this.startSilenceTimer(); // Start silence detection
  }

  stopVoiceRecognition() {
    this.isListening = false;
    this.clearSilenceTimer(); // Clean up timer
  }

  ask() {
    const question = this.newQuestion.trim();
    if (!question) return;

    // Add user message to history
    this.chatHistoryService.addUserMessage(question);
    this.messages = this.chatHistoryService.getMessages();

    this.newQuestion = '';
    this.isTyping = true;

    // Get AI response
    this.chatService.askQuestion(question).subscribe({
      next: res => {
        this.isTyping = false;
        // Add AI response to history
        this.chatHistoryService.addAIMessage(res.answer);
        this.messages = this.chatHistoryService.getMessages();
        this.aiStatus = res.aiStatus;
      },
      error: err => {
        this.isTyping = false;
        const errorMsg = "AI service unavailable. Please try again later.";
        // Add error message to history
        this.chatHistoryService.addAIMessage(errorMsg);
        this.messages = this.chatHistoryService.getMessages();
        this.aiStatus = "Service offline";
        console.error(err);
      }
    });
  }
}