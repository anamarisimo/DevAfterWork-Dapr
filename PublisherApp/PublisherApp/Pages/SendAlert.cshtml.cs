using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PublisherApp.Pages
{
    public class SendAlertModel : PageModel
    {
        [BindProperty]
        public SendAlertViewModel? Alert { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Handle form submission logic here
            // For example, you can save the inputs or send them to an endpoint

            return RedirectToPage(); // Redirect to the same page after submission
        }
    }

    public class SendAlertViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Severity { get; set; }
    }
}
