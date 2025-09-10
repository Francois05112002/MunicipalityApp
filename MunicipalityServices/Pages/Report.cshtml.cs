// MunicipalityServices/Pages/Report.cshtml.cs
namespace MunicipalityServices.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.ComponentModel.DataAnnotations;
    using MunicipalityServices.Models;
    using MunicipalityServices.Data;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class ReportModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReportModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required, StringLength(256)]
        public string Location { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public IssueCategory? Category { get; set; }

        [BindProperty]
        [Required]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } =
            Enum.GetValues(typeof(IssueCategory))
                .Cast<IssueCategory>()
                .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() });

        // --- Pulse Question Logic ---
        public PulseQuestion TodayPulseQuestion { get; set; }

        private static readonly Queue<PulseQuestion> PulseQuestions = new Queue<PulseQuestion>(new[]
        {
            new PulseQuestion { Question = "How satisfied are you with municipal services today?", Answers = new[] { "Very Satisfied", "Satisfied", "Dissatisfied", "Very Dissatisfied" } },
            new PulseQuestion { Question = "How do you rate the cleanliness of public spaces?", Answers = new[] { "Excellent", "Good", "Fair", "Poor" } },
            new PulseQuestion { Question = "How responsive is the municipality to your concerns?", Answers = new[] { "Very Responsive", "Responsive", "Unresponsive", "Very Unresponsive" } },
            new PulseQuestion { Question = "How would you rate local road maintenance?", Answers = new[] { "Excellent", "Good", "Fair", "Poor" } },
            new PulseQuestion { Question = "How safe do you feel in your neighborhood?", Answers = new[] { "Very Safe", "Safe", "Unsafe", "Very Unsafe" } },
            new PulseQuestion { Question = "How satisfied are you with water supply services?", Answers = new[] { "Very Satisfied", "Satisfied", "Dissatisfied", "Very Dissatisfied" } },
            new PulseQuestion { Question = "How would you rate the reliability of electricity?", Answers = new[] { "Very Reliable", "Reliable", "Unreliable", "Very Unreliable" } },
            new PulseQuestion { Question = "How do you find the public transport services?", Answers = new[] { "Excellent", "Good", "Fair", "Poor" } },
            new PulseQuestion { Question = "How would you rate the communication from the municipality?", Answers = new[] { "Excellent", "Good", "Fair", "Poor" } },
            new PulseQuestion { Question = "How satisfied are you with waste collection?", Answers = new[] { "Very Satisfied", "Satisfied", "Dissatisfied", "Very Dissatisfied" } }
        });

        public void OnGet()
        {
            // Pick a new question every 24 hours based on the day since a fixed date
            int dayIndex = (int)(DateTime.UtcNow - new DateTime(2025, 1, 1)).TotalDays % PulseQuestions.Count;
            TodayPulseQuestion = PulseQuestions.ElementAt(dayIndex);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (this.Category == null)
            {
                ModelState.AddModelError(nameof(Category), "The category is required.");
                return Page();
            }

            var issue = new Issue
            {
                Location = this.Location,
                Category = this.Category.Value,
                Description = this.Description,
                // Handle image saving if needed
            };

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                issue.UserId = userId.Value;
            }

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            TempData["ReportSuccess"] = "You successfully reported an Issue within your community";
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostPulseAsync(string selectedAnswer)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var user = await _context.Users.FindAsync(userId.Value);
                if (user != null)
                {
                    user.PulseCompletedCount++;
                    await _context.SaveChangesAsync();
                }
            }
            // ... handle thank you, etc.
            return Page();
        }

    }
}

