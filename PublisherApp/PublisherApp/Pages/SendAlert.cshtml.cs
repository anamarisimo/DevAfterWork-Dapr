using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PublisherApp.Pages
{
    public class SendAlertModel : PageModel
    {
        private readonly DaprClient _dapr;

        public SendAlertModel(DaprClient daprClient)
        {
            _dapr = daprClient;
        }

        [BindProperty]
        public SendAlertViewModel? Alert { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            await _dapr.PublishEventAsync("dapr-pubsub", "my-topic", Alert);

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
