using ConsumerApp.Events;
using Dapr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ConsumerApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DaprEventController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public DaprEventController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
      
        [Topic("pubsub", "my-topic")]
        [HttpPost("event-handler")]
        public async Task<IActionResult> HandleEvent([FromBody] dynamic payload)
        {
            Console.WriteLine($"Received event: {payload}");

            // Send a message to all connected clients (i.e., the frontend)
            await _hubContext.Clients.All.SendAsync("ReceiveEvent", "Event consumed!");

            return Ok();
        }
    }
}
