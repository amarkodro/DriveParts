using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class User : UserAccount
    {
        [ForeignKey(nameof(Gender))]
        public int? GenderId { get; set; }
        public Gender Gender { get; set; }

      
    }
}
