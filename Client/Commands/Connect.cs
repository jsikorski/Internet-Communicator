using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Client.Services;
using Common.Contacts;

namespace Client.Commands
{
    public class Connect : ICommand<string>
    {
        private readonly IServerConnection _serverConnection;

        public Connect(IServerConnection serverConnection)
        {
            _serverConnection = serverConnection;
        }

        public void Execute(string serverAddress)
        {
            try
            {
                _serverConnection.Connect(serverAddress);
            }
            catch (Exception)
            {
                throw new Exception("Cannot connect to server.");
            }
        }
    }
}
