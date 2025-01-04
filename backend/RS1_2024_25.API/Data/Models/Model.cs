using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Model
    {
        public int ModelId { get; set; } 
        public string Name { get; set; }
        public int Year { get; set; }

        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }
        public Car Car { get; set; }

        [ForeignKey(nameof(Engine))]
        public int EngineId { get; set; }
        public Engine Engine { get; set; }


    }
}
