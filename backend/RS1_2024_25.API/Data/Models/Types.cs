using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.Models
{
    public class Types
    {
        [Key]
        public int TypeId { get; set; }
        public string Name { get; set; }
    }
}
