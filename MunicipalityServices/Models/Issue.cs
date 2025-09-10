using System.ComponentModel.DataAnnotations;

namespace MunicipalityServices.Models
{
    public class Issue
    {
        [Key]
        public int IssueId { get; set; }

        [Required, StringLength(256)]
        public string Location { get; set; } = default!;

        [Required]
        public IssueCategory Category { get; set; }

        [Required]
        public string Description { get; set; } = default!;

        // store the saved file path/URL (the upload IFormFile lives in your ViewModel, not here)
        [StringLength(512)]
        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional FK (allow anonymous issues)
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
