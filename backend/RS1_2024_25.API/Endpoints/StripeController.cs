using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using Stripe.Checkout;

namespace RS1_2024_25.API.Endpoints
{

    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;

        public StripeController(IConfiguration configuration, ApplicationDbContext db)
        {
            _configuration = configuration;
            _db = db;
            Stripe.StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession([FromBody] StripeOrderItemRequest order)
        {
            try
            {
                var lineItems = new List<SessionLineItemOptions>();

                foreach (var item in order.Items)
                {
                    
                    var part = _db.Parts.Find(item.PartId);
                    if (part == null)
                        return BadRequest($"Part {item.PartId} not found.");

                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "bam",
                            UnitAmount = (long)(part.Price * 100), 
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = part.Name
                            }
                        },
                        Quantity = item.Quantity
                    });
                }

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = _configuration["Stripe:SuccessUrl"],
                    CancelUrl = _configuration["Stripe:CancelUrl"]
                };

                var service = new SessionService();
                Session session = service.Create(options);

                return Ok(new { sessionId = session.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Payment session creation failed." });
            }
        }
    }

    public class StripeOrderItem
    {
        public int PartId { get; set; }  
        public int Quantity { get; set; }
       
    }

    public class StripeOrderItemRequest
    {
        public List<StripeOrderItem> Items { get; set; }
    }
}

    

    
    


