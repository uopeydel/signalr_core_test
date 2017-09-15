using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalr01.Hubs
{
    public class Chat : HubWithPresence
    {
        //private static List<UserDetails> UserMap = new List<UserDetails>();
        //doSigNalR
        public Chat(IUserTracker<Chat> userTracker)
            : base(userTracker)
        {
        }

        //WEB : Start doSigNalR 003 ////doSigNalR01
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

        /// <summary>
        /// Send2 == methodName
        /// 
        /// //"Send" who subscript this can hear this
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        //public async Task Send2(string[] message, string name)
        //{
        //    //await Clients.All.InvokeAsync("Send", Context.User.Identity.Name, message);
        //    //
        //    var User = new UserDetails(message[1], name);
        //    UserMap.Add(User);

        //    await Clients.Group("Send").InvokeAsync("SendInvoke", message[0]);
        //}

        public async Task SendClients(string[] message)
        {
            //await Clients.All.InvokeAsync("Send", Context.User.Identity.Name, message);
            //
            await Clients.Client(message[0]).InvokeAsync(message[1], "message" + message);
        }

        public async Task GroupsAddAsync(string connectionId, string groupName)
        {
            //await Clients.All.InvokeAsync("Send", Context.User.Identity.Name, message);
            //

            await Groups.AddAsync(connectionId, groupName);
        }

    }
}