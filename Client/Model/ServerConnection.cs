using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client.Model
{
    public class ServerConnection : IServerConnection
    {
        private const int ServerPortNumber = 3000;
        private TcpClient _server;

        public void Connect(string serverAddress)
        {
            _server = new TcpClient();
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), ServerPortNumber);
            _server.Connect(ipEndPoint);
        }
    }
}
