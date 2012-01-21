using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Messages;
using Client.Services;

namespace Client.Commands.User
{
    public class Logout : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly IEventAggregator _eventAggregator;

        public Logout(
            IServerConnection serverConnection, 
            IEventAggregator eventAggregator)
        {
            _serverConnection = serverConnection;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            _serverConnection.Disconnect();
            _eventAggregator.Publish(new LoggedOut());
        }
    }
}
