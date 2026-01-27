import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ChatSignalRService, ChatMessage } from '../services/chat-signalr.service';
import { AuthService } from '../services/auth-services/auth.service';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-user-support-chat',
  templateUrl: './user-support-chat.component.html',
  styleUrls: ['./user-support-chat.component.css']
})
export class UserSupportChatComponent implements OnInit, OnDestroy {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;

  isOpen = false;
  isMinimized = false;
  messages: ChatMessage[] = [];
  newMessage = '';
  isConnected = false;
  isLoggedIn = false;
  isAdmin = false;
  private messageSubscription?: Subscription;

  constructor(
    private chatService: ChatSignalRService,
    private authService: AuthService,
    private http: HttpClient
  ) { }

  async ngOnInit() {
  const token = this.authService.getTokenUser();
  this.isLoggedIn = !!token;

  const userInfo = this.authService.getUserInfoFromToken();
  this.isAdmin = userInfo?.role === 'Admin' || userInfo?.IsAdmin === true;

  console.log('🔍 User Support Chat - Token:', !!token, 'isAdmin:', this.isAdmin);

  if (token && !this.isAdmin) {
    await this.chatService.startConnection(token);
    this.isConnected = this.chatService.isConnected();

    // CRITICAL: Load existing messages FIRST
    await this.loadChatHistory();

    // Then subscribe to new messages
    this.messageSubscription = this.chatService.message$.subscribe(message => {
      if (message) {
        // Avoid duplicates
        const exists = this.messages.some(m => 
          m.messageId === message.messageId ||
          (m.content === message.content && 
           Math.abs(new Date(m.timestamp).getTime() - new Date(message.timestamp).getTime()) < 1000)
        );
        
        if (!exists) {
          this.messages.push(message);
          setTimeout(() => this.scrollToBottom(), 100);
        }
      }
    });
  }

    // Subscribe to login status changes
    this.authService.loginStatus$.subscribe(status => {
    this.isLoggedIn = status;
    const userInfo = this.authService.getUserInfoFromToken();
    this.isAdmin = userInfo?.role === 'Admin' || userInfo?.IsAdmin === true;

    if (status && !this.isConnected && !this.isAdmin) {
      const newToken = this.authService.getTokenUser();
      if (newToken) {
        this.chatService.startConnection(newToken).then(() => {
          this.isConnected = this.chatService.isConnected();
          this.loadChatHistory();
        });
      }
    }
  });
}

 async loadChatHistory() {
  try {
    console.log('📥 Loading chat history...');
    const messages = await this.http.get<ChatMessage[]>(
      'http://localhost:7000/api/SupportChat/user-messages'
    ).toPromise();
    
    if (messages && messages.length > 0) {
      this.messages = messages;
      console.log('✅ Loaded', messages.length, 'messages');
      setTimeout(() => this.scrollToBottom(), 100);
    } else {
      console.log('ℹ️ No previous messages found');
    }
  } catch (err: any) {
    console.error('❌ Error loading chat history:', err);
    if (err.status === 401) {
      console.error('Unauthorized - token might be invalid');
    }
  }
}

  ngOnDestroy() {
    this.messageSubscription?.unsubscribe();
  }

  toggleChat() {
    this.isOpen = !this.isOpen;
    if (this.isOpen) {
      this.isMinimized = false;
      setTimeout(() => this.scrollToBottom(), 100);
    }
  }

  toggleMinimize() {
    this.isMinimized = !this.isMinimized;
  }

  async sendMessage() {
    if (!this.newMessage.trim() || !this.isConnected) return;

    const messageContent = this.newMessage;
    const userId = this.authService.getUserId();
    const userInfo = this.authService.getUserInfoFromToken();

    // Add message to local display immediately (optimistic update)
    const localMessage: ChatMessage = {
      messageId: Date.now(), // Temporary ID
      conversationId: 0, // Will be set by server
      senderId: userId,
      senderName: userInfo ? `${userInfo.name} ${userInfo.surname}` : 'You',
      content: messageContent,
      timestamp: new Date(),
      isFromUser: true
    };

    this.messages.push(localMessage);
    this.newMessage = '';
    setTimeout(() => this.scrollToBottom(), 100);

    // Send to server
    await this.chatService.sendMessageToAdmins(messageContent);
  }

  scrollToBottom(): void {
    try {
      this.messagesContainer.nativeElement.scrollTop =
        this.messagesContainer.nativeElement.scrollHeight;
    } catch (err) { }
  }
}
