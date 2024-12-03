using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.Models
{
    public class Manufacturer
    {
        [Key]
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }

      
    }
}
