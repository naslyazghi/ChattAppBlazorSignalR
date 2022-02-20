using Microsoft.AspNetCore.SignalR;

namespace ChattAppBlazorSignalR.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            string userName = Context.GetHttpContext().Request.Query["userName"];
            Users.Add(Context.ConnectionId, userName);
            await AddMessageToChat(string.Empty, $"{userName} joined the chat!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string userName = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await AddMessageToChat(string.Empty, $"{userName} left!");
        }

        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
