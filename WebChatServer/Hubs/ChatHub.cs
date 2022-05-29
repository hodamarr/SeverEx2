using System.Threading.Tasks;
using WebChatServer.Models;
using Microsoft.AspNetCore.SignalR;
namespace WebChatServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderId, string recieverId)
        {
            await Clients.All.SendAsync("RecieveMessage", senderId, recieverId);
        }
        //public async Task SendContact()
        //{
        //    await Clients.All.SendAsync("invitation");
        //}
    }
}
