using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Contacts;
using Protocol;
using Protocol.Connection;
using Protocol.FileTransfer;
using Protocol.Login;
using Protocol.Messages;
using Protocol.Register;
using Protocol.Statuses;

namespace Client.Services
{
    public class ServerConnection : IServerConnection, IDisposable
    {
        private TcpClient _server;
        private NetworkStream _serverStream;

        private readonly IFormatter _formatter;

        private bool _isConnected;
        public bool IsConnected { get { return _isConnected; } }

        public ServerConnection()
        {
            _formatter = new BinaryFormatter();
        }

        public void Connect(string serverAddress)
        {
            try
            {
                var serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), Ports.ServerListeningPort);
                _server = new TcpClient();
                _server.Connect(serverEndPoint);
                _serverStream = _server.GetStream();
                SendRequest(new ConnectionRequest());
                _isConnected = true;
            }
            catch (Exception)
            {
                throw new Exception("Cannot connect to server.");
            }
        }

        public LoginResponse SendLoginRequest(IRequest loginRequest)
        {
            return (LoginResponse)SendAndGet(loginRequest);
        }

        public RegisterResponse SendRegisterRequest(IRequest registerRequest)
        {
            return (RegisterResponse)SendAndGet(registerRequest);
        }

        public StatusesResponse SendStatusesRequest(IRequest statusesRequest)
        {
            return (StatusesResponse)SendAndGet(statusesRequest);
        }

        public MessageResponse SendMessageRequest(IRequest messageRequest)
        {
            return (MessageResponse)SendAndGet(messageRequest);
        }

        public MessagesResponse SendMessagesRequest(IRequest messagesRequest)
        {
            return (MessagesResponse)SendAndGet(messagesRequest);
        }

        public FileUploadResponse SendFileUploadRequest(IRequest uploadRequest)
        {
            return (FileUploadResponse)SendAndGet(uploadRequest);
        }

        public FilesDownloadResponse SendFilesDownloadRequest(IRequest filesDownloadRequest)
        {
            return (FilesDownloadResponse)SendAndGet(filesDownloadRequest);
        }

        public FileDownloadResponse SendFileDownloadRequest(IRequest fileDownloadRequest)
        {
            return (FileDownloadResponse) SendAndGet(fileDownloadRequest);
        }

        public ConferenceMessageResponse SendConferencialMessageRequest(IRequest messageRequest)
        {
            return (ConferenceMessageResponse) SendAndGet(messageRequest);
        }

        private IResponse SendAndGet(IRequest request)
        {
            lock (_serverStream)
            {
                SendRequest(request);
                return GetResponse();
            }
        }

        private void SendRequest(IRequest request)
        {
            try
            {
                _formatter.Serialize(_serverStream, request);
            }
            catch (Exception)
            {
                throw new Exception("Cannot send request to server.");
            }
        }

        private IResponse GetResponse()
        {
            IResponse response;
            try
            {
                response = (IResponse)_formatter.Deserialize(_serverStream);
            }
            catch (Exception)
            {
                throw new Exception("Cannot get response from server.");
            }

            return response;
        }
        
        public void Disconnect()
        {
            _server.Close();
            _isConnected = false;
        }

        public void Dispose()
        {
            _server.Close();
            _serverStream.Dispose();
        }
    }
}
