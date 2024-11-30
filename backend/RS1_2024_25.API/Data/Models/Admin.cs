using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Admin
    {
        public int AdminId { get; set; } 
        public string Name { get; set; } 
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }


        
    
    }
}
