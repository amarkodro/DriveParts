import {MyConfig} from '../my-config';
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
      return;
    }

    // Stop existing connection if any
    if (this.hubConnection) {
      await this.hubConnection.stop();
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(MyConfig.api_address + '/chathub', {
        accessTokenFactory: () => token,  // Pass JWT token
        skipNegotiation: false,
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection.on('ReceiveMessage', (message: ChatMessage) => {
      message.timestamp = new Date(message.timestamp);
      this.messageSubject.next(message);
    });

    this.hubConnection.onreconnected(() => {
    });

    this.hubConnection.onreconnecting(() => {
    });

    this.hubConnection.onclose(() => {
    });

    try {
      await this.hubConnection.start();
    } catch (err) {
      console.error('❌ SignalR Connection Error:', err);
      throw err;
    }
  }

  async sendMessageToAdmins(content: string, fileUrl?: string, fileName?: string): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.hubConnection.invoke('SendMessageToAdmins', content, fileUrl, fileName);
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
    }
  }
}