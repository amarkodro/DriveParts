using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace RS1_2024_25.API.Endpoints
{

    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        public StripeController()
        {
            Stripe.StripeConfiguration.ApiKey = "sk_test_51RBGYXR0PL10ni1FNUh02XRIZ4HLl0SLWYI3PPi3pREaDi7I72T6WqOssx3uEXBt5sAEugIuUPWXr7Y4iPawIi3j00bppEb3cw";
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
                    SuccessUrl = "http://localhost:4200/order-success",
                    CancelUrl = "http://localhost:4200/checkout"
                };

                var service = new SessionService();
                Session session = service.Create(options);

                return Ok(new { sessionId = session.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
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
