using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace WebApi.Hubs
{
    public class DemoHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            Console.WriteLine(Context.ConnectionId);
            await Clients.Caller.SendAsync("Welcome", "Welcome in signalR");
        }

        public async Task JoinToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("TextMessage", $"New member in group {groupName}: {Context.ConnectionId}");
        }

        public Task SayHelloToOthers(string message)
        {
            return Clients.Others.SendAsync("TextMessage", message);
        }
    }

}
