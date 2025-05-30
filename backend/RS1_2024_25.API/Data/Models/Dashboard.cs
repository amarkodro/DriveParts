namespace RS1_2024_25.API.Data.Models
{

    public class DashboardStats
    {
        public int DashboardId { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int FailedOrders { get; set; }
        public int ApprovedOrders { get; set; }
        public int RejectedOrders { get; set; }
        public int InProgressOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int OnHoldOrders { get; set; }
        public int DraftOrders { get; set; }
        public int SubmittedOrders { get; set; }
        public int TotalCustomers { get; set; }

        public decimal TotalSales { get; set; }
    }

}
