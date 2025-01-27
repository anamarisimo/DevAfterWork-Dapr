using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PublisherApp.Models;

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
        public Alert? Alert { get; set; }

        public readonly IList<string> Regions = new List<string>()
        {
            "Bucuresti",
            "Cluj",
            "Timisoara",
            "Iasi",
            "Craiova"
        };

        public void OnGet()
        {
            ViewData["Regions"] = Regions;
        }

        public async Task<IActionResult> OnPost()
        {
            await _dapr.PublishEventAsync("dapr-pubsub", "my-topic", Alert);

            return RedirectToPage(); // Redirect to the same page after submission
        }
    }
}
