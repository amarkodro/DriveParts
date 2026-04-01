using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(nameof(Status))]
        public int StatusId { get; set; }
        public Status Status { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public UserAccount User { get; set; }

        [ForeignKey(nameof(Supplier))]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [ForeignKey(nameof(Payment))]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        [ForeignKey(nameof(PromoCode))]
        public int? PromoCodeId { get; set; }
        public PromoCode? PromoCode { get; set; }

        public decimal? TotalAmount { get; set; }

        public List<OrderItem> Items { get; set; }

        public string? StripeSessionId { get; set; }

    }
}
