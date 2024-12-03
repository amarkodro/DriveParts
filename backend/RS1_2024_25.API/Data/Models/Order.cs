using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime Date {  get; set; }

        [ForeignKey(nameof(Status))]
        public int StatusId { get; set; }
        public Status Status { get; set; }

        [ForeignKey(nameof (User))]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Supplier))]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [ForeignKey(nameof(Payment))]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }



    }
}
