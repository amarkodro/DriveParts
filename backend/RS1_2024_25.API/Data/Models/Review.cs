using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Part))]
        public int PartId { get; set; }
        public Part Part { get; set; }

        public string Text { get; set; }
        public byte[]? Picture { get; set; }

    }
}
