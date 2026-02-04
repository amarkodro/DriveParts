import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

export interface ChatMessage {
  messageId: number;
  conversationId: number;
  senderId: number;
  senderName: string;
  content: string;
  timestamp: Date;
  isFromUser: boolean;
  fileUrl?: string;
  fileName?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ChatSignalRService {
  private hubConnection?: signalR.HubConnection;
  private messageSubject = new BehaviorSubject<ChatMessage | null>(null);
  public message$ = this.messageSubject.asObservable();

  async startConnection(token: string): Promise<void> {
    // If already connected, don't reconnect
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      console.log('✅ Already connected to SignalR');
      return;
    }

    // Stop existing connection if any
    if (this.hubConnection) {
      await this.hubConnection.stop();
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:7000/chathub', {
        accessTokenFactory: () => token,  // Pass JWT token
        skipNegotiation: false,
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection.on('ReceiveMessage', (message: ChatMessage) => {
      console.log('📨 SignalR received:', message);
      message.timestamp = new Date(message.timestamp);
      this.messageSubject.next(message);
    });

    this.hubConnection.onreconnected(() => {
      console.log('🔄 SignalR reconnected');
    });

    this.hubConnection.onreconnecting(() => {
      console.log('⏳ SignalR reconnecting...');
    });

    this.hubConnection.onclose(() => {
      console.log('❌ SignalR connection closed');
    });

    try {
      await this.hubConnection.start();
      console.log('✅ SignalR Connected successfully');
    } catch (err) {
      console.error('❌ SignalR Connection Error:', err);
      throw err;
    }
  }

  async sendMessageToAdmins(content: string, fileUrl?: string, fileName?: string): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.hubConnection.invoke('SendMessageToAdmins', content, fileUrl, fileName);
        console.log('✅ Message sent to admins');
      } catch (err) {
        console.error('❌ Error sending message:', err);
      }
    } else {
      console.error('❌ Not connected to SignalR');
    }
  }

  async sendMessageToUser(userId: number, content: string, fileUrl?: string, fileName?: string): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.hubConnection.invoke('SendMessageToUser', userId, content, fileUrl, fileName);
        console.log('✅ Message sent to user', userId);
      } catch (err) {
        console.error('❌ Error sending message:', err);
      }
    } else {
      console.error('❌ Not connected to SignalR');
    }
  }

  async markMessagesAsRead(conversationId: number): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      await this.hubConnection.invoke('MarkMessagesAsRead', conversationId);
    }
  }

  isConnected(): boolean {
    return this.hubConnection?.state === signalR.HubConnectionState.Connected;
  }

  async stopConnection(): Promise<void> {
    if (this.hubConnection) {
      await this.hubConnection.stop();
      console.log('🛑 SignalR connection stopped');
    }
  }
}