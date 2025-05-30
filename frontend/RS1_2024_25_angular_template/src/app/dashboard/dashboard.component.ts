import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  stats: any = {};
  chartLabels: string[] = ['Failed', 'Approved', 'Rejected', 'In Progress', 'Cancelled', 'On Hold', 'Draft', 'Submitted','Pending','Completed'];
  chartColors: string[] = ['#FF4C4C', '#28A745', '#FFC107', '#17A2B8', '#6C757D', '#007BFF', '#6610F2', '#E83E8C','#FFC0CB', '#32CD32'];

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.dashboardService.getDashboardStats().subscribe(data => {
      this.stats = data;
      this.createOrderChart();
    });
  }

  createOrderChart(): void {
    new Chart('orderChart', {
      type: 'pie',
      data: {
        labels: this.chartLabels,
        datasets: [{
          data: [
            this.stats.failedOrders,
            this.stats.approvedOrders,
            this.stats.rejectedOrders,
            this.stats.inProgressOrders,
            this.stats.cancelledOrders,
            this.stats.onHoldOrders,
            this.stats.draftOrders,
            this.stats.submittedOrders,
            this.stats.pendingOrders,
            this.stats.completedOrders
          ],
          backgroundColor: this.chartColors
        }]
      },
      options: {
        plugins: {
          legend: {
            display: false // This will hide the labels at the top
          }
        }
      }
    });
  }
}
