import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { OrdersService,Order } from './admin-orders.service';
describe('OrdersService', () => {
  let service: OrdersService;
  let httpMock: HttpTestingController;

  // Mock data matching your API response
  const mockOrders: Order[] = [
    {
      orderId: 1,
      date: new Date('2023-09-01'),
      statusName: 'pending',
      username: 'john_doe',
      supplierName: 'Supplier A',
      paymentMethod: 'Credit Card',
      statusId:1
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [OrdersService]
    });
    service = TestBed.inject(OrdersService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Verify no outstanding requests
  });

  it('should fetch orders', () => {
    service.getOrders().subscribe(orders => {
      expect(orders).toEqual(mockOrders); // Now compares valid data
    });

    // Mock the HTTP request
    const req = httpMock.expectOne('api/orders');
    expect(req.request.method).toBe('GET');
    
    // Respond with mock data
    req.flush(mockOrders);
  });
});