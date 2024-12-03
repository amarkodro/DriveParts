using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public bool IsCard { get; set; }
        public bool IsCash { get; set; }
    }
}
