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
            public int? PromoCodeId { get; set; }
            public decimal? TotalAmount { get; set; }
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
            public string? PromoCode { get; set; }
            public float? Discount { get; set; }
            public decimal? TotalAmount { get; set; }

        }

        public class OrderItemRequest
        {
            public int PartId { get; set; }
            public int Quantity { get; set; }
            public long Price { get; set; }
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
                                PromoCode = x.PromoCode != null ? x.PromoCode.Code : "Unknown",
                                Discount = x.PromoCode != null ? x.PromoCode.Discount : 0,
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
                                PromoCode = x.PromoCode != null ? x.PromoCode.Code : "Unknown",
                                Discount = x.PromoCode != null ? x.PromoCode.Discount : 0,
                            }).First();

            return order;

        }

        [HttpPost("add")]
        public ActionResult<OrderResponse> PostOrder(OrderRequest request)
        {

            if (request.PromoCodeId.HasValue)
            {
                var promo = _db.PromoCodes.Find(request.PromoCodeId.Value);
                if (promo == null)
                    return BadRequest("Invalid promo code.");
            }

            var order = new Order
            {
                Date = DateTime.Now,
                StatusId = 1,
                UserId = request.UserId,
                SupplierId = request.SupplierId,
                PaymentId = 1,
                PromoCodeId = request.PromoCodeId,
                TotalAmount = request.TotalAmount,
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
                    Price = (long)item.Price,               
                };

                _db.OrderItems.Add(orderItem);
            }

            var cartItems = _db.CartItems.Where(x => x.UserId == request.UserId).ToList();

            if (cartItems.Any())
            {
                _db.CartItems.RemoveRange(cartItems);
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
            order.PromoCodeId = request.PromoCodeId;

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
