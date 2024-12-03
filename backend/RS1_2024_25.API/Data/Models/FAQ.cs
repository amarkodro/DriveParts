using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class FAQ
    {
        public int FAQId { get; set; }

        public string Question { get; set; }
        public string  Answer { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        

    }
}
