using AzTUChat.DAL;
using AzTUChat.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AzTUChat.Hubs
{
    public class ChatHub:Hub
    {
        public AppDbContext _context { get; }

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }
        public Task SendMessage( string message)
        {
            return Clients.All.SendAsync("ReceiveMessage",  message);
        }
        public override Task OnConnectedAsync()
        {
            AppUser user = _context.Users.SingleOrDefault(x => x.UserName == Context.User.Identity.Name);
             _context.SaveChanges();
            Clients.All.SendAsync("Connected", Context.User.Identity.Name);
            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            AppUser user = _context.Users.SingleOrDefault(x => x.UserName == Context.User.Identity.Name);
            user.ConnectionId = null;
            _context.SaveChanges();
            Clients.All.SendAsync("Connected", Context.User.Identity.Name);
            return Task.CompletedTask;
        }
    }
}
