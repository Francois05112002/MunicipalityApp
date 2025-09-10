// MunicipalityServices/Pages/Report.cshtml.cs
namespace MunicipalityServices.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.ComponentModel.DataAnnotations;
    using MunicipalityServices.Models;
    using MunicipalityServices.Data; // Add this using
    using System.Threading.Tasks;

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

        public void OnGet()
        {
            // Categories already set above, or set here if you prefer
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
                Category = this.Category.Value, // Safe to access Value because of the null check above
                Description = this.Description,
                // Handle image saving if needed
            };

            // Associate issue with signed-in user if available
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
    }
}
