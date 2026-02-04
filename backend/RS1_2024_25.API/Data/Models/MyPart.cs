using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Data.Models
{
    public class MyPart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public UserAccount User { get; set; }

        [ForeignKey(nameof(Part))]
        public int PartId { get; set; }
        public Part Part { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
