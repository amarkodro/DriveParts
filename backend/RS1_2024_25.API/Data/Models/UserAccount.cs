using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool isUser {  get; set; }
        public bool? is2FActive { get; set; }
        public string? ImageUrl { get; set; }

        [ForeignKey(nameof(City))]
        public int? CityId { get; set; }
        public City City { get; set; }
    }
}
