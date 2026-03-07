
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Journey
    {
        public int ID { get; set; }

        public string Name { get; set; }


        [ForeignKey(nameof(StartCity))]
        public int StartCityId { get; set; }
        public City? StartCity { get; set; }


        [ForeignKey(nameof(EndCity))]
        public int EndCityId { get; set; }
        public City? EndCity { get; set; }
    }
}
