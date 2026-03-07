import { MyConfig } from '../my-config';
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
    this.http.get<any[]>(MyConfig.api_address + '/api/SupportChat/conversations').subscribe({
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
    this.http.get<ChatMessage[]>(`${MyConfig.api_address}/api/SupportChat/messages/${conversationId}`).subscribe({
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
      const exists = conversation.messages.some(m =>
        m.messageId === message.messageId ||
        (m.content === message.content &&
          Math.abs(new Date(m.timestamp).getTime() - new Date(message.timestamp).getTime()) < 1000)
      );
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
      this.loadConversations();
    }
  }

  selectedFile: File | null = null;
  isUploading = false;

  async sendMessage() {
    if ((!this.newMessage.trim() && !this.selectedFile) || !this.selectedConversation || !this.isConnected) return;

    let fileUrl: string | undefined;
    let fileName: string | undefined;

    if (this.selectedFile) {
      this.isUploading = true;
      try {
        const uploadResult = await this.uploadFile(this.selectedFile);
        fileUrl = uploadResult.url;
        fileName = uploadResult.fileName;
      } catch (err) {
        console.error('Upload failed', err);
        this.isUploading = false;
        return; // Stop sending if upload failed
      }
      this.isUploading = false;
    }

    const messageContent = this.newMessage;
    const userId = this.authService.getUserId();
    const userInfo = this.authService.getUserInfoFromToken();

    // No longer adding message locally (signalr will broadcast it back to us)
    this.newMessage = '';
    this.selectedFile = null;
    // SignalR will handle adding the message via handleIncomingMessage
    // as admins receive all broadcasts in the 'Admins' group.

    // Send to server
    await this.chatService.sendMessageToUser(this.selectedConversation.userId, messageContent, fileUrl, fileName);
  }

  onFileDropped(files: FileList) {
    if (files && files.length > 0) {
      this.selectedFile = files[0];
    }
  }

  onFileSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      this.selectedFile = event.target.files[0];
    }
  }

  removeFile() {
    this.selectedFile = null;
  }

  private uploadFile(file: File): Promise<{ url: string, fileName: string }> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<{ url: string, fileName: string }>(MyConfig.api_address + '/api/storage/upload', formData)
      .toPromise()
      .then(res => res!); // Non-null assertion for simplicity, handle properly in prod
  }

  scrollToBottom(): void {
    try {
      this.messagesContainer.nativeElement.scrollTop =
        this.messagesContainer.nativeElement.scrollHeight;
    } catch (err) { }
  }
}