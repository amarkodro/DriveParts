namespace RS1_2024_25.API.Data.Models
{
    public class PromoCodeUsage
    {
        public int Id { get; set; }
        public int PromoCodeId { get; set; }
        public PromoCode PromoCode { get; set; }
        public int UserId { get; set; }
        public UserAccount User { get; set; }
        public DateTime UsedAt { get; set; }
    }
}
