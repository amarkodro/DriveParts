using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("stats")]
        public IActionResult GetDashboardStats()
        {
            var totalSales = _db.OrderItems
                .Where(oi => oi.Order.Status.Name == "Completed") // Filter by the 'Completed' order status
                .Sum(oi => (decimal)(oi.Quantity * oi.Price)); // Explicitly cast the result to decimal

            var dashboardStats = new DashboardStats
            {
                TotalOrders = _db.Orders.Count(),
                PendingOrders = _db.Orders.Count(o => o.Status.Name == "Pending"),
                CompletedOrders = _db.Orders.Count(o => o.Status.Name == "Completed"),
                FailedOrders = _db.Orders.Count(o => o.Status.Name == "Failed"),
                ApprovedOrders = _db.Orders.Count(o => o.Status.Name == "Approved"),
                RejectedOrders = _db.Orders.Count(o => o.Status.Name == "Rejected"),
                InProgressOrders = _db.Orders.Count(o => o.Status.Name == "In Progress"),
                CancelledOrders = _db.Orders.Count(o => o.Status.Name == "Cancelled"),
                OnHoldOrders = _db.Orders.Count(o => o.Status.Name == "On Hold"),
                DraftOrders = _db.Orders.Count(o => o.Status.Name == "Draft"),
                SubmittedOrders = _db.Orders.Count(o => o.Status.Name == "Submitted"),
                TotalCustomers = _db.Users.Count(),
                TotalSales = totalSales

            };

            return Ok(dashboardStats);
        }

        
    }
}