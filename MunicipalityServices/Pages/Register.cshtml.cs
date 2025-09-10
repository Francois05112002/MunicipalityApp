using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using MunicipalityServices.Models;
using MunicipalityServices.Data;
using Microsoft.EntityFrameworkCore;

namespace MunicipalityServices.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegisterModel(AppDbContext context)
        {
            _context = context;
        }

        // Registration properties
        [BindProperty]
        [Required, StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [BindProperty]
        [Required, StringLength(32)]
        [Phone]
        public string UserPhone { get; set; } = string.Empty;

        // Sign-in property
        [BindProperty]
        public string? SignInPhone { get; set; }

        // User state properties
        public string? CurrentUserName { get; set; }
        public string? CurrentUserPhone { get; set; }
        public int IssueCount { get; set; }
        public int PulseCompletedCount { get; set; } 


        public async Task OnGetAsync()
        {
            await LoadUserStateAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = new User
            {
                UserName = this.UserName,
                UserPhone = this.UserPhone
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set session for signed-in state
            HttpContext.Session.SetString("SignedIn", "true");
            HttpContext.Session.SetInt32("UserId", user.UserId);

            TempData["RegisterSuccess"] = "Account created successfully!";
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostSignInAsync()
        {
            if (string.IsNullOrWhiteSpace(SignInPhone))
            {
                ModelState.AddModelError("SignInPhone", "Phone number is required.");
                await LoadUserStateAsync();
                return Page();
            }

            var user = await _context.Users
                .Include(u => u.Issues)
                .FirstOrDefaultAsync(u => u.UserPhone == SignInPhone);

            if (user == null)
            {
                ModelState.AddModelError("SignInPhone", "No account found with this phone number.");
                await LoadUserStateAsync();
                return Page();
            }

            // Set session for signed-in state
            HttpContext.Session.SetString("SignedIn", "true");
            HttpContext.Session.SetInt32("UserId", user.UserId);

            TempData["SignInSuccess"] = "Signed in successfully!";
            return RedirectToPage("Index");
        }

        private async Task LoadUserStateAsync()
        {
            var signedIn = HttpContext.Session.GetString("SignedIn");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (signedIn == "true" && userId.HasValue)
            {
                var user = await _context.Users.Include(u => u.Issues).FirstOrDefaultAsync(u => u.UserId == userId.Value);
                if (user != null)
                {
                    CurrentUserName = user.UserName;
                    CurrentUserPhone = user.UserPhone;
                    IssueCount = user.Issues.Count;
                }
            }
        }
    }
}



