using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.Models
{
    public class Engine
    {
  
        public int EngineId { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public double Displacement { get; set; }
        public string FuelType { get; set; }

       
    }
}
