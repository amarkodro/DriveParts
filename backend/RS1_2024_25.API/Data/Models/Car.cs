using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Car
    {

        public int CarId { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }

        
    }
}
