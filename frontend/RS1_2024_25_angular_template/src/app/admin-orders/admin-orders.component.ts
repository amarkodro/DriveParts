import { Component, OnInit } from '@angular/core';
import { OrdersService,Order } from '../services/admin-orders.service';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'app-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  paginatedOrders: Order[] = [];
  pageSize = 5  ;
  currentPage = 1;
  totalPages = 1;
   searchTerm = '';
   selectedStatusId = 0;
filteredOrders: Order[] = [];  // holds all filtered orders
  statusOptions = [
    { id: 1, name: 'Pending' },    // Must match seeded names
    { id: 2, name: 'Approved' },  // Exactly as in your database
    { id: 3, name: 'Rejected' },
    { id: 4, name: 'In Progress' },
    { id: 5, name: 'Completed' },
    { id: 6, name: 'Cancelled' },
    { id: 7, name: 'On Hold' },
    { id: 8, name: 'Failed' },
    { id: 9, name: 'Draft' },
    { id: 10, name: 'Submitted' },
  ];
isDownloading = false;
  constructor(private ordersService: OrdersService,private snackBar:MatSnackBar) {}

  ngOnInit(): void {
    this.loadOrders();
  }
 loadOrders(): void {
  this.ordersService.getOrders().subscribe(apiOrders => {
    this.orders = apiOrders.map(order => {
      const normalize = (s: string) => s.trim().toLowerCase().replace(/[^a-z]/g, '');
      const statusMatch = this.statusOptions.find(s =>
        normalize(s.name) === normalize(order.statusName || '')
      );
      return {
        ...order,
        statusId: statusMatch?.id || 0
      };
    });

    this.applyFilters();
  });
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
  this.pageSize = Number(this.pageSize); // Force conversion to number
  this.currentPage = 1;
  this.applyFilters();
}
 updateOrderStatus(orderId: number, statusId: number): void {
  if (!statusId || statusId === 0) {
    alert('Invalid status!');
    return;
  }
  
  this.ordersService.updateOrderStatus(orderId, statusId).subscribe({
    next: () => {
      this.loadOrders(); // Refresh the list
      alert('Status updated!'); // Temporary feedback
    },
    error: (err) => {
      console.error('Update failed:', err);
      alert('Update failed. Check console.');
    }
  });
}
  updateStatus(orderId: number, statusId: number): void {
    this.ordersService.updateOrderStatus(orderId, +statusId).subscribe(() => {
      this.loadOrders(); // Refresh data
    });
  }
   deleteOrder(orderId: number): void {
    if (confirm('Are you sure you want to delete this order?')) {
      this.ordersService.deleteOrder(orderId).subscribe({
        next: () => {
          this.loadOrders(); // Refresh the list after deletion
        },
        error: (err) => {
          console.error('Failed to delete order:', err);
        }
      });
    }
  }
  downloadPdf(orderId: number): void {
    this.isDownloading = true;
    this.ordersService.downloadReceipt(orderId).subscribe({
      next: (blob: Blob) => {
        // Create a temporary URL for the Blob
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Receipt_${orderId}.pdf`; // Set filename
        a.click(); // Trigger download
        window.URL.revokeObjectURL(url); // Clean up

        this.isDownloading = false;
        this.snackBar.open('Receipt downloaded!', 'Close', { duration: 3000 });
      },
      error: (error) => {
        this.isDownloading = false;
        this.snackBar.open('Failed to download receipt.', 'Close', { duration: 3000 });
        console.error('Download error:', error);
      }
    });
  }

  onSearchChange(): void {
  this.applyFilters();
}
  // Add status filter handler
 onStatusFilterChange(): void {
  this.selectedStatusId = +this.selectedStatusId;
  this.applyFilters();
}
  applyFilters(): void {
  this.filteredOrders = this.orders.filter(order => {
    const statusMatches = this.selectedStatusId === 0 || order.statusId === this.selectedStatusId;
    const searchMatches = !this.searchTerm || order.orderId.toString().includes(this.searchTerm);
    return statusMatches && searchMatches;
  });

  // Ensure division uses numbers
  this.totalPages = Math.max(1, Math.ceil(this.filteredOrders.length / this.pageSize));
  this.currentPage = 1;
  this.updatePaginatedOrders();
}

updatePaginatedOrders(): void {
  const startIndex = (this.currentPage - 1) * this.pageSize;
  const endIndex = startIndex + this.pageSize;
  this.paginatedOrders = this.filteredOrders.slice(startIndex, endIndex);

  console.log(
    `Filtered ${this.filteredOrders.length} orders, page ${this.currentPage}/${this.totalPages}, showing ${this.paginatedOrders.length}`
  );
}
}