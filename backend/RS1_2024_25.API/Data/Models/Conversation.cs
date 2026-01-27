using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime LastMessageAt { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
