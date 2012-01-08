using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Protocol;
using Protocol.Login;
using Protocol.Messages;
using Protocol.Register;
using Protocol.Statuses;

namespace Server
{
    class Server
    {
        private readonly IFormatter _formatter;
        private readonly TcpListener _tcpListener;
        private readonly Thread _listenThread;
        private readonly Dictionary<int, NetworkStream> _activeConnections;

        public Server()
        {
            _formatter = new BinaryFormatter();
            _tcpListener = new TcpListener(IPAddress.Any, Ports.ServerListeningPort);
            _activeConnections = new Dictionary<int, NetworkStream>();
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
            var tcpClient = (TcpClient)client;
            var clientStream = tcpClient.GetStream();
            var clientNumber = 0;

            // first request always must be ConnectionRequest
            var request = GetRequest(clientStream);
            if (request.ToString() == "Protocol.Connection.ConnectionRequest")
            {
                Console.Out.WriteLine("Connection from " + tcpClient.Client.RemoteEndPoint);
            }
            else
            {
                return;
            }

            // waiting for sucessfull logon or successfull registration
            // after three unsuccessfull logon trys server disconnects
            var times = 1;
            while (true)
            {
                request = GetRequest(clientStream);
                LogRequest(request, tcpClient.Client.RemoteEndPoint);

                if (request.ToString() == "Protocol.Login.LoginRequest")
                {
                    var loginRequest = (LoginRequest)request;

                    // temporary mockup
                    clientNumber = loginRequest.Number;
                    _activeConnections.Add(loginRequest.Number, clientStream);
                    SendReponse(clientStream, new LoginResponse() { WasSuccessfull = true });
                    // end

                    break;
                }
                else if (request.ToString() == "Protocol.Register.RegisterRequest")
                {
                    SendReponse(clientStream, new RegisterResponse() { AccountNumber = 123, WasSuccessfull = true });
                    times = 1;
                }

                if (times == 3)
                {
                    tcpClient.Close();
                    return;
                }

                times++;
            }

            // main service
            while (true)
            {
                request = GetRequest(clientStream);
                LogRequest(request, tcpClient.Client.RemoteEndPoint, clientNumber);

                if (request.ToString() == "Protocol.FileTransfer.FileUploadRequest")
                {
                    // TODO FileUploadRequest
                }
                else if (request.ToString() == "Protocol.Messages.MessageRequest")
                {
                    var messageRequest = (MessageRequest)request;
                    var messageResponse = new MessageResponse
                    {
                        ReceiversNumbers = messageRequest.ReceiversNumbers,
                        Sender = messageRequest.Sender,
                        Text = messageRequest.Text
                    };


                    foreach (var receiver in messageRequest.ReceiversNumbers)
                    {
                        var connection = _activeConnections[receiver];
                        SendReponse(connection, messageResponse);
                    }
                }
                else if (request.ToString() == "Protocol.Statuses.StatusesRequest")
                {
                    var statusRequest = (StatusesRequest)request;
                    var statusResponse = new StatusesResponse {Contacts = statusRequest.Contacts};

                    foreach (var contact in statusResponse.Contacts)
                    {
                        contact.IsAvailable = _activeConnections.Any(ac => ac.Key == contact.ContactStoredData.Number);
                    }

                    SendReponse(clientStream, statusResponse);
                }
                else if (request.ToString() == "Protocol.Login.LogoutRequest")
                {
                    _activeConnections.Remove(clientNumber);
                    tcpClient.Close();
                    break;
                }
                else
                {
                    Console.Out.WriteLine("Something went wrong :(");
                }
            }
        }

        private void LogRequest(IRequest request, EndPoint remoteEndPoint)
        {
            Console.Out.WriteLine(request.ToString() + " from " + remoteEndPoint);
        }

        private void LogRequest(IRequest request, EndPoint remoteEndPoint, int clientNumber)
        {
            Console.Out.WriteLine(request.ToString() + " from user " + clientNumber + " at " + remoteEndPoint);
        }

        private void SendReponse(Stream stream, IResponse response)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _formatter.Serialize(stream, response);
        }

        private IRequest GetRequest(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            return (IRequest)_formatter.Deserialize(stream);
        }


    }
}
