using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common.Files;
using Common.Messages;
using Protocol;
 
namespace Server
{
    class Server
    {
        private readonly TcpListener _tcpListener;
        private readonly Thread _listenThread;
        private readonly Dictionary<int, NetworkStream> _activeConnections;
        private readonly Dictionary<int, List<Message>> _messages;
        private readonly Dictionary<int, List<ConferenceMessage>> _conferenceMessages;
        private readonly Dictionary<int, List<GuidedFile>> _files;

        public Server()
        {
            _tcpListener = new TcpListener(IPAddress.Any, Ports.ServerListeningPort);
            _activeConnections = new Dictionary<int, NetworkStream>();
            _messages = new Dictionary<int, List<Message>>();
            _files = new Dictionary<int, List<GuidedFile>>();
            _listenThread = new Thread(new ThreadStart(ListenForClients));
            _listenThread.Start();
        }

        private void ListenForClients()
        {
            _tcpListener.Start();

            while (true)
            {
                // blocks until a client has connected to the server
                var client = _tcpListener.AcceptTcpClient();

                // create a thread to handle communication 
                // with connected client
                var clientThread = new Thread(new ParameterizedThreadStart(ClientCommunicationHandler));
                clientThread.Start(client);
            }
        }

        private void ClientCommunicationHandler(object client)
        {
            new ClientCommunication((TcpClient)client, _activeConnections, _messages, _files, _conferenceMessages);
        }
    }
}
