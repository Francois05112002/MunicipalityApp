using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MunicipalityServices.Models
{
    [Index(nameof(UserPhone), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(32)]
        [Phone] // basic format check; you can customize later for SA numbers
        public string UserPhone { get; set; } = default!;

        [Required, StringLength(100)]
        public string UserName { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        //public UserPoints? Points { get; set; }
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    }
}
