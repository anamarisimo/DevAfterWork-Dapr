using ConsumerApp.Events;
using ConsumerApp.Models;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ConsumerApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DaprEventController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly DaprClient _daprClient;

        public DaprEventController(IHubContext<NotificationHub> hubContext, DaprClient daprClient)
        {
            _hubContext = hubContext;
            _daprClient = daprClient;
        }

        [Topic("dapr-pubsub", "my-topic")]
        [HttpPost("event-handler")]
        public async Task<IActionResult> HandleEvent([FromBody] Alert payload)
        {
            if (!IsValidPayload(payload, out var errorMessage))
            {
                return BadRequest(errorMessage);
            }

            try
            {
                Console.WriteLine($"Processing event for region: {payload.Region}, type: {payload.Type}");

                // Get phone numbers (either from cache or service)
                var phoneNumbers = await GetPhoneNumbersForRegionAsync(payload.Region);

                // Concatenate phone numbers for display
                var concatenatedResponse = string.Join(", ", phoneNumbers);
                Console.WriteLine($"Phone numbers for region {payload.Region}: {concatenatedResponse}");

                // Notify via SignalR
                await NotifyClientsAsync(payload.Region, concatenatedResponse, payload.Type.ToString());

                return Ok(new { Message = "Event processed successfully", PhoneNumbers = concatenatedResponse });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing event: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the event.");
            }
        }

        private bool IsValidPayload(Alert payload, out string errorMessage)
        {
            errorMessage = null;
            if (payload == null || string.IsNullOrWhiteSpace(payload.Region))
            {
                errorMessage = "Invalid payload. Region is required.";
                return false;
            }
            return true;
        }

        private async Task<List<string>> GetPhoneNumbersForRegionAsync(string region)
        {
            var cacheKey = $"phoneNumbers_{region}";

            // Check if data exists in cache
            var cachedPhoneNumbers = await _daprClient.GetStateAsync<List<string>>("statestore", cacheKey);
            if (cachedPhoneNumbers != null && cachedPhoneNumbers.Count > 0)
            {
                Console.WriteLine($"Cache hit for region {region}");
                return cachedPhoneNumbers;
            }

            // Cache miss: Fetch phone numbers from service
            Console.WriteLine($"Cache miss for region {region}. Fetching from service...");
            var phoneNumbers = (await _daprClient.InvokeMethodAsync<IEnumerable<string>>(
                HttpMethod.Get,
                "inforetrievalservice",
                $"api/phonenumbers?region={region}"
            )).ToList();

            // Cache the phone numbers for 15 minutes
            await CachePhoneNumbersAsync(cacheKey, phoneNumbers);

            return phoneNumbers;
        }

        private async Task CachePhoneNumbersAsync(string cacheKey, List<string> phoneNumbers)
        {
            await _daprClient.SaveStateAsync("statestore", cacheKey, phoneNumbers, 
                metadata: new Dictionary<string, string>() {["ttlInSeconds"] =  "900"});
            Console.WriteLine($"Cached phone numbers for key: {cacheKey}");
        }

        private async Task NotifyClientsAsync(string region, string phoneNumbers, string alertType)
        {
            string message = $"Sending SMS to {region}: {phoneNumbers}";
            await _hubContext.Clients.All.SendAsync("ReceiveEvent", message, alertType);
            Console.WriteLine($"Notified clients: {message}");
        }
    }
}
