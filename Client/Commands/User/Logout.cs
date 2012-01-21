using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Context;
using Client.Messages;
using Client.Services;

namespace Client.Commands.User
{
    public class Logout : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly IEventAggregator _eventAggregator;
        private readonly ICurrentContext _currentContext;

        public Logout(
            IServerConnection serverConnection, 
            IEventAggregator eventAggregator,
            ICurrentContext currentContext)
        {
            _serverConnection = serverConnection;
            _eventAggregator = eventAggregator;
            _currentContext = currentContext;
        }

        public void Execute()
        {
            _serverConnection.Disconnect();
            _currentContext.LoggedUserNumber = 0;
            _eventAggregator.Publish(new LoggedOut());
        }
    }
}
