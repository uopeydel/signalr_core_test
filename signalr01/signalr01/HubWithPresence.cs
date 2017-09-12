using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalr01
{
    public class HubWithPresence : Hub
    {
        private IUserTracker<HubWithPresence> _userTracker;

        //User 2 Join.. 001 ////User 2 out.. 001
        public HubWithPresence(IUserTracker<HubWithPresence> userTracker)
        {
            _userTracker = userTracker;
        }

        //User 2 Join.. 002
        public Task<IEnumerable<UserDetails>> GetUsersOnline()
        {
            return _userTracker.UsersOnline();
        }

        public virtual Task OnUsersJoined(UserDetails[] user)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnUsersLeft(UserDetails[] user)
        {
            return Task.CompletedTask;
        }
    }
}
