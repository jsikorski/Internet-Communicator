using System;
using Caliburn.Micro;
using Client.Messages;
using Client.Services;

namespace Client.Commands.Connection
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
