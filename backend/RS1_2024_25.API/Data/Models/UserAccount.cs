using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool isUser {  get; set; }
        public bool is2FActive { get; set; }

        [ForeignKey(nameof(Admin))]
        public int? AdminId { get; set; }
        public Admin? Admin { get; set; }

        [ForeignKey(nameof(User))]

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
