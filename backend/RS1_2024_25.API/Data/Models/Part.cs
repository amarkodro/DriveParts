using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Part
    {
        public int PartId { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey(nameof(Manufacturer))]

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

       public string? PartImage { get; set; }

        public bool IsFeatured { get; set; } = false;
        public bool IsOnSale { get; set; } = false;
        public bool IsNewArrival { get; set; } = false;

        [ForeignKey(nameof(Types))]

        public int? TypeId { get; set; }
        public Types? Type { get; set; }

    }
}
