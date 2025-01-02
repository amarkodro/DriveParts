using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class ModelPart
    {
        public int ModelPartId  { get; set; }

        [ForeignKey(nameof(Model))]
        public int ModelId { get; set; }
        public Model Model { get; set; }

        [ForeignKey(nameof(Part))]
        public int PartId { get; set; }
        public Part Part { get; set; }


    }
}