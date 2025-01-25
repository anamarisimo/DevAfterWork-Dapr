using Microsoft.AspNetCore.Mvc;

namespace YourAppNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneNumbersController : ControllerBase
    {
        // Simulated data for phone numbers grouped by region
        private static readonly Dictionary<string, List<string>> _regionPhoneNumbers = new()
        {
            { "Bucuresti", new List<string> { "+40712345678", "+40798765432", "+40754321098", "+40765432109" } },
            { "Cluj", new List<string> { "+40711223344", "+40799887766", "+40744556677", "+40755667788" } },
            { "Timisoara", new List<string> { "+40755667788", "+40733445566", "+40722334455", "+40766778899" } },
            { "Iasi", new List<string> { "+40722334455", "+40766778899", "+40788990011", "+40744556677" } },
            { "Constanta", new List<string> { "+40788990011", "+40744556677", "+40722334455", "+40766778899" } }
        };

        [HttpGet]
        public IActionResult GetPhoneNumbersByRegion([FromQuery] string region)
        {
            if (string.IsNullOrWhiteSpace(region))
            {
                return BadRequest("Region parameter is required.");
            }

            // Normalize region input (case insensitive matching)
            var normalizedRegion = region.Trim();
            var phoneNumbers = _regionPhoneNumbers
                .Where(kvp => kvp.Key.Equals(normalizedRegion, System.StringComparison.OrdinalIgnoreCase))
                .SelectMany(kvp => kvp.Value)
                .ToList();

            if (!phoneNumbers.Any())
            {
                return NotFound($"No phone numbers found for the region: {region}");
            }

            return Ok(phoneNumbers);
        }
    }
}
