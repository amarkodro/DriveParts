using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool isRevoked { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;

        [ForeignKey(nameof(UserAccount))]
        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
