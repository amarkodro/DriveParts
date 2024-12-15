using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class CarPart
    {
        public int CarPartId { get; set; }

        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }
        public Car Car { get; set; }

        [ForeignKey(nameof(Part))]
        public int PartId { get; set; }
        public Part Part { get; set; }


    }
}