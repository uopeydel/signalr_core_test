using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalr01.Hubs
{
    public class Chat : HubWithPresence
    {
        //doSigNalR
        public Chat(IUserTracker<Chat> userTracker)
            : base(userTracker)
        {
        }

        ////doSigNalR01
        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).InvokeAsync("SetUsersOnline", await GetUsersOnline());

            await base.OnConnectedAsync();
        }

        //user2 join
        public override Task OnUsersJoined(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("UsersJoined", new[] { users });
        }

        //user2 left
        public override Task OnUsersLeft(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("UsersLeft", new[] { users });
        }

        //WEB : Click SendMessage 001
        //Do this method 'Send' before 'PresenceHubLifetimeManager.InvokeAllAsync'
        public async Task Send(string message)
        {
            await Clients.All.InvokeAsync("Send", Context.User.Identity.Name, message);
        }
    }
}