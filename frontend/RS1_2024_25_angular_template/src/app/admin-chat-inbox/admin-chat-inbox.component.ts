import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ChatSignalRService, ChatMessage } from '../services/chat-signalr.service';
import { AuthService } from '../services/auth-services/auth.service';
import { HttpClient } from "@angular/common/http";
import { Subscription } from 'rxjs';

interface Conversation {
  id: number;
  userId: number;
  userName: string;
  lastMessage: string;
  lastMessageAt: Date;
  unreadCount: number;
  messages: ChatMessage[];
}

@Component({
  selector: 'app-admin-chat-inbox',
  templateUrl: './admin-chat-inbox.component.html',
  styleUrls: ['./admin-chat-inbox.component.css']
})
export class AdminChatInboxComponent implements OnInit, OnDestroy {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;

  conversations: Conversation[] = [];
  selectedConversation: Conversation | null = null;
  newMessage = '';
  isConnected = false;
  private messageSubscription?: Subscription;

  constructor(
    private chatService: ChatSignalRService,
    private authService: AuthService,
    private http: HttpClient
  ) { }

  async ngOnInit() {
    const token = this.authService.getTokenUser();
    if (token) {
      await this.chatService.startConnection(token);
      this.isConnected = this.chatService.isConnected();

      this.loadConversations();

      this.messageSubscription = this.chatService.message$.subscribe(message => {
        if (message) {
          this.handleIncomingMessage(message);
        }
      });
    }
  }

  ngOnDestroy() {
    this.messageSubscription?.unsubscribe();
  }

  loadConversations() {
    this.http.get<any[]>('http://localhost:7000/api/SupportChat/conversations').subscribe({
      next: (data) => {
        this.conversations = data.map(c => ({
          id: c.id,
          userId: c.userId,
          userName: c.userName,
          lastMessage: c.lastMessage,
          lastMessageAt: new Date(c.lastMessageAt),
          unreadCount: c.unreadCount,
          messages: []
        }));
      },
      error: (err) => console.error('Error loading conversations:', err)
    });
  }

  selectConversation(conversation: Conversation) {
    this.selectedConversation = conversation;
    this.loadMessages(conversation.id);
    this.chatService.markMessagesAsRead(conversation.id);
    conversation.unreadCount = 0;
  }

  loadMessages(conversationId: number) {
    this.http.get<ChatMessage[]>(`http://localhost:7000/api/SupportChat/messages/${conversationId}`).subscribe({
      next: (messages) => {
        if (this.selectedConversation) {
          this.selectedConversation.messages = messages;
          setTimeout(() => this.scrollToBottom(), 100);
        }
      },
      error: (err) => console.error('Error loading messages:', err)
    });
  }

  handleIncomingMessage(message: ChatMessage) {
  console.log('📨 Admin received message:', message);

  let conversation = this.conversations.find(c => c.id === message.conversationId);

  if (conversation) {
    // Update conversation
    conversation.lastMessage = message.content;
    conversation.lastMessageAt = message.timestamp;

    // CRITICAL FIX: Initialize messages array if not exist
    if (!conversation.messages) {
      conversation.messages = [];
    }

    // Avoid duplicates
    const exists = conversation.messages.some(m => m.messageId === message.messageId);
    if (!exists) {
      conversation.messages.push(message);
    }

    // If this is the selected conversation, scroll
    if (this.selectedConversation?.id === conversation.id) {
      // Update the selected conversation reference
      this.selectedConversation.messages = conversation.messages;
      setTimeout(() => this.scrollToBottom(), 100);
    } else {
      // Only increment unread if not currently viewing
      conversation.unreadCount++;
    }

    // Move to top
    this.conversations = [
      conversation,
      ...this.conversations.filter(c => c.id !== conversation.id)
    ];
  } else {
    // New conversation - reload the entire list
    console.log('🔄 New conversation detected, reloading...');
    this.loadConversations();
  }
}

  async sendMessage() {
    if (!this.newMessage.trim() || !this.selectedConversation || !this.isConnected) return;

    const messageContent = this.newMessage;
    const userId = this.authService.getUserId();
    const userInfo = this.authService.getUserInfoFromToken();

    // Add message to local display immediately (optimistic update)
    const localMessage: ChatMessage = {
      messageId: Date.now(), // Temporary ID
      conversationId: this.selectedConversation.id,
      senderId: userId,
      senderName: userInfo ? `${userInfo.name} ${userInfo.surname}` : 'Admin',
      content: messageContent,
      timestamp: new Date(),
      isFromUser: false
    };

    this.selectedConversation.messages.push(localMessage);
    this.newMessage = '';
    setTimeout(() => this.scrollToBottom(), 100);

    // Send to server
    await this.chatService.sendMessageToUser(this.selectedConversation.userId, messageContent);
  }

  scrollToBottom(): void {
    try {
      this.messagesContainer.nativeElement.scrollTop =
        this.messagesContainer.nativeElement.scrollHeight;
    } catch (err) { }
  }
}
