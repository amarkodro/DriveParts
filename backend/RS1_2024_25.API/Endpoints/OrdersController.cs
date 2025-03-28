using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using static RS1_2024_25.API.Endpoints.UsersController;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(ApplicationDbContext _db) : ControllerBase
    {
        public class OrderRequest
        {
            public DateTime Date { get; set; }
            public int StatusId { get; set; }     
            public int UserId { get; set; }         
            public int SupplierId { get; set; }                      
            public int PaymentId { get; set; }
            public List<OrderItemRequest> Items { get; set; }

        }

        public class OrderResponse
        {
            public int OrderId { get; set; }
            public DateTime Date { get; set; }
            public string StatusName { get; set; }
            public string Username { get; set; }
            public string SupplierName { get; set; }
            public string PaymentMethod { get; set; }
        }
        
        public class OrderItemRequest
        {
            public int PartId { get; set; }
            public int Quantity { get; set; }
            public float Price { get; set; }
        }

        [HttpGet]
        public ActionResult<OrderResponse[]> GetOrders()
        {
            var orders = _db.Orders
                            .Include(x => x.Status)
                            .Include(x => x.User)
                            .Include(x => x.Supplier)
                            .Include(x => x.Payment)
                            .Select(x => new OrderResponse
                            {
                                OrderId = x.OrderId,
                                Date = x.Date,
                                StatusName = x.Status != null ? x.Status.Name : "Unknown",
                                Username = x.User != null ? x.User.Username : "Unknown",
                                SupplierName = x.Supplier != null ? x.Supplier.Name : "Unknown",
                                PaymentMethod = x.Payment != null ? x.Payment.PaymentMethod : "Unknown",
                            }).ToArray();

            return orders;

        }


        [HttpGet("{id}")]
        public ActionResult<OrderResponse> GetOrder(int id)
        {
            var order = _db.Orders
                            .Include(x => x.Status)
                            .Include(x => x.User)
                            .Include(x => x.Supplier)
                            .Include(x => x.Payment).Where(x=>x.OrderId == id)
                            .Select(x => new OrderResponse
                            {
                                OrderId = x.OrderId,
                                Date = x.Date,
                                StatusName = x.Status != null ? x.Status.Name : "Unknown",
                                Username = x.User != null ? x.User.Username : "Unknown",
                                SupplierName = x.Supplier != null ? x.Supplier.Name : "Unknown",
                                PaymentMethod = x.Payment != null ? x.Payment.PaymentMethod : "Unknown",
                            }).First();

            return order;

        }

        [HttpPost]
        public ActionResult<OrderResponse> PostOrder(OrderRequest request)
        {
            var order = new Order
            {
                Date = DateTime.Now,
                StatusId = request.StatusId,
                UserId = request.UserId,
                SupplierId = request.SupplierId,
                PaymentId = request.PaymentId,
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            foreach(var item in request.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    PartId = item.PartId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                _db.OrderItems.Add(orderItem);
            }

            _db.SaveChanges();
            var response = new OrderResponse
            { 
                Date = order.Date,
                Username=_db.Users.Find(order.UserId)?.Username ?? "Unknown",
                SupplierName=_db.Suppliers.Find(order.SupplierId)?.Name ?? "Unknown",
                StatusName=_db.Statuses.Find(order.StatusId)?.Name ?? "Unknown",
                PaymentMethod=_db.Payments.Find(order.PaymentId)?.PaymentMethod ?? "Unknown",
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<string> PutOrder(int id, OrderRequest request)
        {
            var order = _db.Orders.Find(id) ?? throw new KeyNotFoundException("Order not found");

            order.UserId = request.UserId;
            order.SupplierId = request.SupplierId;
            order.StatusId = request.StatusId;
            order.PaymentId = request.PaymentId;
             
            _db.SaveChanges();

            return Ok("Order updated successfully");
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeleteOrder(int id)
        {
            var order = _db.Orders.Find(id) ?? throw new KeyNotFoundException("Order not found");

            _db.Orders.Remove(order);
            _db.SaveChanges();

            return Ok("Order deleted successfully");
        }

    }
}
