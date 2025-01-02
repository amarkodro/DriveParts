using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class PartEngine
    {
        public int PartEngineId { get; set; }
        [ForeignKey(nameof(Part))]
        public int PartId { get; set; }
        public Part Part { get; set; }

        [ForeignKey(nameof(Engine))]
        public int EngineId { get; set; }
        public Engine Engine { get; set; }


    }
}
