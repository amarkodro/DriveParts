using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Part
    {
        public int PartId { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }
        public int Description { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey(nameof(Car))]

        public int CarId { get; set; }
        public Car Car { get; set; }

        [ForeignKey(nameof(Manufacturer))]

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}
