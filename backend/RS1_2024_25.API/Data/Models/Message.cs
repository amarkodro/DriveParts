using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Conversation))]
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }
        public UserAccount Sender { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }

        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }

        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
    }
}
