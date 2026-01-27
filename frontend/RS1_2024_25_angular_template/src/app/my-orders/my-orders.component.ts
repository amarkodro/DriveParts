import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MyOrdersService, Order } from '../services/my-orders.service';
import { AuthService } from '../services/auth-services/auth.service';
import { OrderService } from '../services/order.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.css'],
   encapsulation: ViewEncapsulation.None
})
export class MyOrdersComponent implements OnInit {
  orders: Order[] = [];
  paginatedOrders: Order[] = [];
  pageSize = 5;
  currentPage = 1;
  totalPages = 1;
  searchTerm = '';
  filteredOrders: Order[] = [];
  isDownloading = false;
  userId: number | null = null;
orderToCancel: Order | null = null;
showCancelModal = false;
  statusOptions = [
    { id: 1, name: 'Pending' },
    { id: 2, name: 'Approved' },
    { id: 3, name: 'Rejected' },
    { id: 4, name: 'In Progress' },
    { id: 5, name: 'Completed' },
    { id: 6, name: 'Cancelled' },
    { id: 7, name: 'On Hold' },
    { id: 8, name: 'Failed' },
    { id: 9, name: 'Draft' },
    { id: 10, name: 'Submitted' },
  ];

  constructor(
    private ordersService: MyOrdersService,
    private authService: AuthService,
    private snackBar: MatSnackBar,
private dialog: MatDialog,
private orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getCurrentUserId();
    if (this.userId) {
      this.loadOrders();
    } else {
      console.error('User not authenticated');
      this.snackBar.open('Please login to view orders', 'Close', { duration: 3000 });
    }
  }

  loadOrders(): void {
    this.ordersService.getOrdersByCustomer(this.userId!).subscribe({
      next: (apiOrders) => {
        this.orders = apiOrders.map(order => ({
          ...order,
          date: new Date(order.date) // Convert string to Date object
        }));
        this.applyFilters();
      },
      error: (err) => {
        console.error('Failed to load orders:', err);
        this.snackBar.open('Failed to load orders', 'Close', { duration: 3000 });
      }
    });
  }
// Add this method to check if order can be canceled
canCancel(statusName: string | undefined): boolean {
  if (!statusName) return false;
  const cancellableStatuses = ['Pending', 'Submitted', 'On Hold', 'Approved'];
  return cancellableStatuses.includes(statusName);
}
// Add these methods for cancel functionality
openCancelModal(order: Order): void {
  this.orderToCancel = order;
  this.showCancelModal = true;
}
closeCancelModal(): void {
  this.showCancelModal = false;
  this.orderToCancel = null;
}
confirmCancel(): void {
  if (this.orderToCancel) {
    this.isDownloading = true; // Show loading state
    this.ordersService.cancelOrder(this.orderToCancel.orderId).subscribe({
      next: () => {
        // Find and update the order in our local array
        const index = this.orders.findIndex(o => o.orderId === this.orderToCancel?.orderId);
        if (index !== -1) {
          this.orders[index].statusName = 'Cancelled';
          this.applyFilters(); // Re-apply filters to update pagination
        }
        
        this.snackBar.open('Order has been cancelled successfully!', 'Close', { duration: 3000 });
        this.closeCancelModal();
        this.isDownloading = false;
      },
      error: (error) => {
        console.error('Error cancelling order:', error);
        this.snackBar.open('Failed to cancel order. Please try again.', 'Close', { duration: 3000 });
        this.closeCancelModal();
        this.isDownloading = false;
      }
    });
  }
}
  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePaginatedOrders();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePaginatedOrders();
    }
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  downloadPdf(orderId: number): void {
    this.isDownloading = true;
    this.ordersService.downloadReceipt(orderId).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Receipt_${orderId}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
        
        this.isDownloading = false;
        this.snackBar.open('Receipt downloaded!', 'Close', { duration: 3000 });
      },
      error: (error) => {
        this.isDownloading = false;
        this.snackBar.open('Failed to download receipt', 'Close', { duration: 3000 });
        console.error('Download error:', error);
      }
    });
  }

  onSearchChange(): void {
    this.applyFilters();
  }

  applyFilters(): void {
    this.filteredOrders = this.orders.filter(order => {
      const searchMatches = !this.searchTerm || 
        order.orderId.toString().includes(this.searchTerm) ||
        (order.supplierName && order.supplierName.toLowerCase().includes(this.searchTerm.toLowerCase()));
      return searchMatches;
    });

    this.totalPages = Math.max(1, Math.ceil(this.filteredOrders.length / this.pageSize));
    this.currentPage = 1;
    this.updatePaginatedOrders();
  }

  updatePaginatedOrders(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedOrders = this.filteredOrders.slice(startIndex, endIndex);
  }

  getStatusClass(statusName: string | undefined): string {
    if (!statusName) return 'status-unknown';
    
    const status = statusName.toLowerCase().replace(/\s+/g, '-');
    return `status-${status}`;
  }
}