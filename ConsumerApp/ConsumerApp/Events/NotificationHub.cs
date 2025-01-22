using Microsoft.AspNetCore.SignalR;

namespace ConsumerApp.Events
{
    public class NotificationHub : Hub
    {
        public async Task SendEventNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveEvent", message);
        }
    }
}
