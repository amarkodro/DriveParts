using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace RS1_2024_25.API.Endpoints
{

    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StripeController(IConfiguration configuration)
        {
            _configuration = configuration;
            Stripe.StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession([FromBody] StripeOrderItemRequest order)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = order.Items.Select(item => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "bam",
                            UnitAmount = item.Price,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name
                            }
                        },
                        Quantity = item.Quantity
                    }).ToList(),
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
        public string Name { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }

    public class StripeOrderItemRequest
    {
         public List<StripeOrderItem> Items { get; set; }
    }
    
}

