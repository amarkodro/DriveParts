import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ChatService } from './ai.service';

describe('ChatService', () => {
  let service: ChatService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ChatService]
    });
    service = TestBed.inject(ChatService);
    httpMock = TestBed.inject(HttpTestingController); // <-- Initialize here
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should send a question and receive an answer', () => {
    const dummyResponse = { answer: 'This is an AI response.' };
    const question = 'What is the capital of France?';

    service.askQuestion(question).subscribe(res => {
      expect(res).toEqual(dummyResponse);
    });

    const req = httpMock.expectOne({
      method: 'POST',
      url: '/api/chat/ask'
    });
    
    expect(req.request.body).toEqual({ question });
    req.flush(dummyResponse);
  });
});