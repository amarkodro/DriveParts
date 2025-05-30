using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static RS1_2024_25.API.Endpoints.UsersController;
using static System.Net.Mime.MediaTypeNames;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Rectangle = iTextSharp.text.Rectangle;
namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(ApplicationDbContext _db) : ControllerBase
    {
        public class OrderRequest
        {
            public DateTime Date { get; set; }

            [Required]
            public int StatusId { get; set; }

            [Required]
            public int UserId { get; set; }

            [Required]
            public int SupplierId { get; set; }

            [Required]
            public int PaymentId { get; set; }
            public int? PromoCodeId { get; set; }
            public decimal? TotalAmount { get; set; }

            [Required]
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
            
        
            [Required]
            [Range(1, int.MaxValue)]
            public int PartId { get; set; }

            [Required]
            [Range(1, 1000)]
            public int Quantity { get; set; }

            [Required]
            [Range(0.01, 1000000)]
            public decimal Price { get; set; }
        }
        public class OrderUpdateRequest
        {
            public int StatusId { get; set; } // Only include updatable fields
        }

        [HttpGet]
        public ActionResult<OrderResponse[]> GetOrders()
        {
            var orders = _db.Orders
                .AsNoTracking()
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
 .AsNoTracking()
     .Include(x => x.Status)
     .Include(x => x.User)
     .Include(x => x.Supplier)
     .Include(x => x.Payment)
     .FirstOrDefault(x => x.OrderId == id); // Filter first

            if (order == null) return NotFound();

            return Ok(new OrderResponse
            {
                OrderId = order.OrderId,
                Date = order.Date,
                StatusName = order.Status?.Name ?? "Unknown",
                Username = order.User?.Username ?? "Unknown",
                SupplierName = order.Supplier?.Name ?? "Unknown",
                PaymentMethod = order.Payment?.PaymentMethod ?? "Unknown"
            });

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

            foreach (var item in request.Items)
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
                Username = _db.Users.Find(order.UserId)?.Username ?? "Unknown",
                SupplierName = _db.Suppliers.Find(order.SupplierId)?.Name ?? "Unknown",
                StatusName = _db.Statuses.Find(order.StatusId)?.Name ?? "Unknown",
                PaymentMethod = _db.Payments.Find(order.PaymentId)?.PaymentMethod ?? "Unknown",
            };

            return Ok(response);
        }

        [HttpPost]
        public ActionResult<OrderResponse> PostOrders(OrderRequest request)
        {
            using var transaction = _db.Database.BeginTransaction();
            // Validate foreign keys
            if (!_db.Users.Any(u => u.Id == request.UserId))
                return BadRequest("Invalid UserId");

            if (!_db.Suppliers.Any(s => s.SupplierId == request.SupplierId))
                return BadRequest("Invalid SupplierId");

            if (!_db.Payments.Any(p => p.PaymentId == request.PaymentId))
                return BadRequest("Invalid PaymentId");
            try
            {
                var order = new Order
                {
                    Date = DateTime.Now,
                    StatusId = request.StatusId,
                    UserId = request.UserId,
                    SupplierId = request.SupplierId,
                    PaymentId = request.PaymentId
                };

                _db.Orders.Add(order);
                _db.SaveChanges(); // Generates OrderId

                foreach (var item in request.Items)
                {
                    _db.OrderItems.Add(new OrderItem
                    {
                        OrderId = order.OrderId, // Use the generated ID
                        PartId = item.PartId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }

                _db.SaveChanges();
                transaction.Commit();

                return Ok(/* Your response */);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        /*[HttpPost]
        public ActionResult<OrderResponse> PostOrder(OrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            return Ok(new OrderResponse
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
                OrderId = order.OrderId,
                Date = order.Date,
                StatusName = _db.Statuses.Find(order.StatusId)?.Name ?? "Unknown",
                Username = _db.Users.Find(order.UserId)?.Username ?? "Unknown",
                SupplierName = _db.Suppliers.Find(order.SupplierId)?.Name ?? "Unknown",
                PaymentMethod = _db.Payments.Find(order.PaymentId)?.PaymentMethod ?? "Unknown"
            });

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
        }*/

        /*[HttpPut("{id}")]
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
        }*/

        [HttpPut("{id}")]
        public ActionResult<string> PutOrder(int id, OrderUpdateRequest request)
        {
            try
            {
                var order = _db.Orders.Find(id);
                if (order == null) return NotFound("Order not found");

                order.StatusId = request.StatusId;
                _db.SaveChanges();
                return Ok(new { message = "Status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeleteOrder(int id)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // Find the order
                var order = _db.Orders.Find(id);
                if (order == null) return NotFound("Order not found");

                // Delete all related OrderItems first
                var orderItems = _db.OrderItems.Where(oi => oi.OrderId == id);
                _db.OrderItems.RemoveRange(orderItems);

                // Now delete the order
                _db.Orders.Remove(order);
                _db.SaveChanges();
                transaction.Commit();

                return Ok(new { message = "Order and related items deleted successfully" });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpGet("GenerateReceipt/{orderId:int}")]
        public IActionResult GenerateReceipt(int orderId)
        {
            Console.WriteLine($"generating reciept for order id: {orderId}");
            try
            {
                var order = _db.Orders
                    .Include(o => o.Status)
                    .Include(o => o.User)
                    .Include(o => o.Supplier)
                    .Include(o => o.Payment)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Part)
                    .FirstOrDefault(o => o.OrderId == orderId);

                if (order == null)
                {
                    Console.WriteLine($"Order {orderId} not found.");
                    return NotFound();
                }
                decimal subtotal = order.Items.Sum(i => i.Price * i.Quantity);
                decimal tax = subtotal * 0.17m;
                decimal total = subtotal + tax;

                using (MemoryStream ms = new MemoryStream())
                {
                    // Explicit iTextSharp references
                    var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 30, 30);
                    var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    // --- Logo ---
                    string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","UserImages", "11c809db-ab93-4353-9b43-f08dc8014cb9.png");
                    if (System.IO.File.Exists(logoPath))
                    {
                        var logo = iTextSharp.text.Image.GetInstance(logoPath);
                        logo.ScaleToFit(100f, 60f);
                        logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        doc.Add(logo);
                    }

                    // --- Header ---
                    var header = new iTextSharp.text.Paragraph("DriveParts\nLacina 55\nMostar 88000\nBosna i Hercegovina",
                        iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10));
                    header.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc.Add(header);

                    doc.Add(new Paragraph("\n"));
                    var orderIdParagraph = new Paragraph($"Order ID: {order.OrderId}",
                        FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
                    doc.Add(orderIdParagraph);

                    // --- Table ---
                    var table = new PdfPTable(4)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 10f,
                        SpacingAfter = 15f
                    };
                    
                    table.SetWidths(new float[] { 50, 15, 15, 20 });

                    // Add headers
                    AddHeaderCell(table, "Item");
                    AddHeaderCell(table, "Qty");
                    AddHeaderCell(table, "Price");
                    AddHeaderCell(table, "Total");

                    foreach (var item in order.Items)
                    {
                        table.AddCell(item.Part?.Name ?? "N/A");
                        table.AddCell(item.Quantity.ToString());
                        table.AddCell(item.Price.ToString("C"));
                        table.AddCell((item.Price * item.Quantity).ToString("C"));
                    }

                    doc.Add(table);

                    // Totals Section
                    var totals = new PdfPTable(2)
                    {
                        WidthPercentage = 40,
                        HorizontalAlignment = 2,
                        SpacingBefore = 10f
                    };
                    totals.DefaultCell.Border = Rectangle.NO_BORDER;

                    AddTotalRow(totals, "Subtotal:", subtotal);
                    AddTotalRow(totals, "Tax (17%):", tax);
                    AddTotalRow(totals, "Total:", total, true);

                    doc.Add(totals);

                    // Signature Section
                    var footerTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 40f
                    };
                    footerTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    // Left Side - Company Director
                    var directorCell = new PdfPCell(new Phrase("Director Signature:\n________________\n\nAmar Kodro\nDirector, DriveParts",
                        FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    directorCell.Border = Rectangle.NO_BORDER;
                    directorCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    footerTable.AddCell(directorCell);

                    // Right Side - Client
                    var clientCell = new PdfPCell(new Phrase("Client Signature:\n________________\n\n" + order.User?.Username,
                        FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    clientCell.Border = Rectangle.NO_BORDER;
                    clientCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    footerTable.AddCell(clientCell);

                    doc.Add(footerTable);

                    doc.Close();
                    return File(ms.ToArray(), "application/pdf", $"Receipt_{order.OrderId}.pdf");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private void AddHeaderCell(PdfPTable table, string text)
        {
            // 1. Create the phrase
            var phrase = new Phrase(
                text,
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)
            );

            // 2. Create the cell and set its border
            var cell = new PdfPCell(phrase)
            {
                Border = Rectangle.BOTTOM_BORDER // ✅ Set border on the cell
            };

            // 3. Add the cell to the table
            table.AddCell(cell);
        }
        private void AddTotalRow(PdfPTable table, string label, decimal value, bool isBold = false)
        {
            var font = isBold ?
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10) :
                FontFactory.GetFont(FontFactory.HELVETICA, 10);

            table.AddCell(new Phrase(label, font));
            table.AddCell(new Phrase(value.ToString("C"), font));
        }
    } 
}
