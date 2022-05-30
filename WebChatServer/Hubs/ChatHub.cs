using System.Threading.Tasks;
using WebChatServer.Models;
using Microsoft.AspNetCore.SignalR;
namespace WebChatServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string sender, string reciever)
        {
            await Clients.All.SendAsync("Recieve", sender, reciever);
        }
    }
}
