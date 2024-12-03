using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey(nameof (Part))]
        public int PartId { get; set; }
        public Part Part { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }

    }
}
