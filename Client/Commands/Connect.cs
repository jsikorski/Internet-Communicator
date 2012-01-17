using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using Client.Messages;
using Client.Services;
using Common.Contacts;

namespace Client.Commands
{
    public class Connect : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly string _serverAddress;
        private readonly IEventAggregator _eventAggregator;

        public Connect(
            IServerConnection serverConnection, 
            string serverAddress, 
            IEventAggregator eventAggregator)
        {
            _serverConnection = serverConnection;
            _serverAddress = serverAddress;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            try
            {
                _serverConnection.Connect(_serverAddress);
            }
            catch (Exception)
            {
                throw new Exception("Cannot connect to server.");
            }

            _eventAggregator.Publish(new Connected());
        }
    }
}
