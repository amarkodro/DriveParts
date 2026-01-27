import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ChatHistoryService {
  private readonly CHAT_HISTORY_KEY = 'ai_chat_history';
  private sessionMessages: { role: string, content: string }[] = [];

  constructor() {
    // Initialize with welcome message
    this.addAIMessage("Hello! I'm your AI assistant. How can I help you today?");
  }

  addUserMessage(content: string) {
    this.sessionMessages.push({ role: 'user', content });
    this.saveSession();
  }

  addAIMessage(content: string) {
    this.sessionMessages.push({ role: 'assistant', content });
    this.saveSession();
  }

  getMessages() {
    return this.sessionMessages;
  }

  clearSession() {
    this.sessionMessages = [];
    sessionStorage.removeItem(this.CHAT_HISTORY_KEY);
  }

  private saveSession() {
    sessionStorage.setItem(this.CHAT_HISTORY_KEY, JSON.stringify(this.sessionMessages));
  }
}