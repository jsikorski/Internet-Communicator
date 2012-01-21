using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Files;
using Common.Messages;
using MySql.Data.MySqlClient;
using Protocol;
using Protocol.FileTransfer;
using Protocol.Login;
using Protocol.Messages;
using Protocol.Register;
using Protocol.Statuses;
 
namespace Server
{
    class ClientCommunication
    {
        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _clientStream;
        private readonly IFormatter _formatter;
        private readonly Dictionary<int, NetworkStream> _activeConnections;
        private Dictionary<int, List<Message>> _messages;
        private Dictionary<int, List<GuidedFile>> _files;
        private List<GuidedFile> _filesToDownload;
        private int _clientNumber = -1;
        private Dictionary<int, List<ConferenceMessage>> _conferenceMessages;

        public ClientCommunication(TcpClient client, Dictionary<int, NetworkStream> connections, Dictionary<int, List<Message>> messages, Dictionary<int, List<GuidedFile>> files, Dictionary<int, List<ConferenceMessage>> conferenceMessages)
        {
            _filesToDownload = new List<GuidedFile>();
            _tcpClient = client;
            _clientStream = _tcpClient.GetStream();
            _activeConnections = connections;
            _formatter = new BinaryFormatter();
            _messages = messages;
            _files = files;
            _conferenceMessages = conferenceMessages;

            Communication();
        }

        private void Communication()
        {
            try
            {
                InitialCommunication();
                LogonAndRegister();
                MainService();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(_tcpClient.Client.RemoteEndPoint + " connection ended. Reason: " + e.Message);
                if (_clientNumber != -1)
                {
                    _activeConnections.Remove(_clientNumber);
                }
                _clientStream.Close();
                _tcpClient.Close();
                return;
            }
        }

        private void InitialCommunication()
        {
            // first request always must be ConnectionRequest
            var request = GetRequest();

            if (request.ToString() == "Protocol.Connection.ConnectionRequest")
            {
                Console.Out.WriteLine("Incoming connection from " + _tcpClient.Client.RemoteEndPoint);
            }
            else
            {
                throw new Exception("First request was not a connection request");
            }
        }

        private void LogonAndRegister()
        {
            var sqlConnection = new MySqlConnection("Server=mysql-stoinska.ogicom.pl;Port=3306;Database=db174700;Uid=db174700;password=rvcID4X4cV");

            // waiting for sucessfull logon or successfull registration
            while (true)
            {
                var request = GetRequest();
                LogRequest(request, _tcpClient.Client.RemoteEndPoint);

                if (request.ToString() == "Protocol.Login.LoginRequest")
                {
                    sqlConnection.Open();
                    var loginRequest = (LoginRequest)request;
                    var command = sqlConnection.CreateCommand();
                    command.CommandText = "SELECT PasswordHash FROM users WHERE Number=" + loginRequest.Number;

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader["PasswordHash"].ToString() == loginRequest.PasswordHash)
                        {
                            _clientNumber = loginRequest.Number;
                            _activeConnections.Add(loginRequest.Number, _clientStream);

                            if (!_messages.ContainsKey(_clientNumber))
                                _messages.Add(_clientNumber, new List<Message>());
                            if (!_conferenceMessages.ContainsKey(_clientNumber))
                                _conferenceMessages.Add(_clientNumber, new List<ConferenceMessage>());
                            if (!_files.ContainsKey(_clientNumber))
                                _files.Add(_clientNumber, new List<GuidedFile>());
                            
                            SendReponse(new LoginResponse() { WasSuccessfull = true });
                            
                            reader.Close();
                            command.Dispose();
                            sqlConnection.Close();
                            break;
                        }
                    }

                    reader.Close();
                    command.Dispose();
                    sqlConnection.Close();

                    SendReponse(new LoginResponse() { WasSuccessfull = false });
                }
                else if (request.ToString() == "Protocol.Register.RegisterRequest")
                {
                    sqlConnection.Open();
                    var registerRequest = (RegisterRequest)request;
                    var command = sqlConnection.CreateCommand();

                    command.CommandText = "INSERT INTO users (Number, PasswordHash) values (null, \"" + registerRequest.Password + "\")";
                    command.ExecuteNonQuery();

                    SendReponse(new RegisterResponse() { AccountNumber = (int)command.LastInsertedId, WasSuccessfull = true });
                    command.Dispose();
                    sqlConnection.Close();
                }
            }
            sqlConnection.Close();
        }

        private void MainService()
        {
            while (true)
            {
                var request = GetRequest();
                LogRequest(request, _tcpClient.Client.RemoteEndPoint, _clientNumber);

                if (request.ToString() == "Protocol.FileTransfer.FileUploadRequest")
                {
                    FileUploadHandler(request);
                }
                else if (request.ToString() == "Protocol.FileTransfer.FileDownloadRequest")
                {
                    FileDownloadHandler(request);
                }
                else if (request.ToString() == "Protocol.FileTransfer.FilesDownloadRequest")
                {
                    FilesDownloadHandler();
                }
                else if (request.ToString() == "Protocol.Messages.MessageRequest")
                {
                    MessageHandler(request);
                }
                else if (request.ToString() == "Protocol.Messages.MessagesRequest")
                {
                    MessagesHandler();
                }
                else if (request.ToString() == "Protocol.Messages.ConferenceMessageRequest")
                {
                    ConferenceMessageHandler(request);
                }
                else if (request.ToString() == "Protocol.Messages.ConferenceMessagesRequest")
                {
                    ConferenceMessagesHandler();
                }
                else if (request.ToString() == "Protocol.Statuses.StatusesRequest")
                {
                    StatusHandler(request); 
                }
                else if (request.ToString() == "Protocol.Login.LogoutRequest")
                {
                    _activeConnections.Remove(_clientNumber);
                    _clientStream.Close();
                    _tcpClient.Close();
                    return;
                }
                else
                {
                    Console.Out.WriteLine("Something went wrong :(");
                }
            }
        }

        private void ConferenceMessagesHandler()
        {
            var messages = _conferenceMessages[_clientNumber];
            _conferenceMessages[_clientNumber] = new List<ConferenceMessage>();
            var response = new ConferenceMessagesResponse(messages);
            SendReponse(response);
        }

        private void ConferenceMessageHandler(IRequest request)
        {
            var messageRequest = (ConferenceMessageRequest)request;
            SendReponse(new ConferenceMessageResponse());

            foreach (var receiver in messageRequest.ReciversNumbers)
            {
                if (!_conferenceMessages.ContainsKey(receiver))
                {
                    _conferenceMessages.Add(receiver, new List<ConferenceMessage>());
                }

                var message = new ConferenceMessage(_clientNumber, 
                    DateTime.Now, messageRequest.Text, messageRequest.ReciversNumbers);
                _conferenceMessages[receiver].Add(message);
            }
        }

        private void FileDownloadHandler(IRequest request)
        {
            var fileDownloadRequest = (FileDownloadRequest) request;
            GuidedFile fileToDownload = null;

            foreach (var file in _filesToDownload)
            {
                // nie wiem ocb ale jest duzo nulli w _f :O
                if (file != null)
                {
                    if (file.Guid.Equals(fileDownloadRequest.FileGuid))
                    {
                        fileToDownload = file;
                    }
                    break;
                }
            }
            _filesToDownload.Remove(fileToDownload);

            var response = new FileDownloadResponse(fileToDownload.File);
            SendReponse(response);
        }

        private void FilesDownloadHandler()
        {
            var files = new List<GuidedFile>();
            var fileHeaders = new List<FileHeader>();

            foreach (var file in _files[_clientNumber])
            {
                files.Add(file);
            }

            foreach (var file in files)
            {
                _files[_clientNumber].Remove(file);
                _filesToDownload.Add(file);
                fileHeaders.Add(new FileHeader(file.Guid, file.File.OriginalName, file.File.SenderNumber));
            }
            
            var response = new FilesDownloadResponse(fileHeaders);
            SendReponse(response);
        }

        private void MessagesHandler()
        {
            var messages = _messages[_clientNumber];
            _messages[_clientNumber] = new List<Message>();
            var response = new MessagesResponse(messages);
            SendReponse(response);
        }

        private void FileUploadHandler(IRequest request)
        {
            var uploadRequest = (FileUploadRequest) request;
            SendReponse(new FileUploadResponse());
            
            if (!_files.ContainsKey(uploadRequest.ReceiverNumber))
            {
                _files.Add(uploadRequest.ReceiverNumber, new List<GuidedFile>());
            }

            var file = new GuidedFile(new File(uploadRequest.OriginalName, uploadRequest.FileBytes, _clientNumber), Guid.NewGuid());
            _files[uploadRequest.ReceiverNumber].Add(file);
        }

        private void MessageHandler(IRequest request)
        {
            var messageRequest = (MessageRequest)request;
            SendReponse(new MessageResponse());

            var receiverNumber = messageRequest.ReciverNumber;

            if (!_messages.ContainsKey(receiverNumber))
            {
                _messages.Add(receiverNumber, new List<Message>());
            }
            
            var message = new Message(_clientNumber, DateTime.Now, messageRequest.Text);
            _messages[receiverNumber].Add(message);
        }

        private void StatusHandler(IRequest request)
        {
            var statusRequest = (StatusesRequest)request;
            var statusResponse = new StatusesResponse { Contacts = statusRequest.Contacts };

            foreach (var contact in statusResponse.Contacts)
            {
                contact.IsAvailable = _activeConnections.Any(ac => ac.Key == contact.ContactStoredData.Number);
            }

            SendReponse(statusResponse);
        }

        private void LogRequest(IRequest request, EndPoint remoteEndPoint)
        {
            Console.Out.WriteLine(request + " from " + remoteEndPoint);
        }

        private void LogRequest(IRequest request, EndPoint remoteEndPoint, int clientNumber)
        {
            Console.Out.WriteLine(request + " from user " + clientNumber + " at " + remoteEndPoint);
        }

        private void SendReponse(IResponse response)
        {
            _formatter.Serialize(_clientStream, response);
        }

        private IRequest GetRequest()
        {
            return (IRequest)_formatter.Deserialize(_clientStream);
        }
    }
}