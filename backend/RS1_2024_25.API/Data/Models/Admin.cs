using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Admin: UserAccount
    {
       public string AdminLevel { get; set; }
    }
}
