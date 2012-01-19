using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Messages;
using MySql.Data.MySqlClient;
using Protocol;
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
        private int _clientNumber = -1;

        public ClientCommunication(TcpClient client, Dictionary<int, NetworkStream> connections, Dictionary<int, List<Message>> messages)
        {
            _tcpClient = client;
            _clientStream = _tcpClient.GetStream();
            _activeConnections = connections;
            _formatter = new BinaryFormatter();
            _messages = messages;

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
                    
                    SendReponse(new RegisterResponse() { AccountNumber = (int) command.LastInsertedId, WasSuccessfull = true });
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
                else if (request.ToString() == "Protocol.Messages.MessageRequest")
                {
                    MessageHandler(request);
                }
                else if (request.ToString() == "Protocol.Messages.MessagesRequest")
                {
                    MessagesHandler();
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

        private void MessagesHandler()
        {
            MessagesResponse response;
            
            if (_messages[_clientNumber] == null)
            {
                response = new MessagesResponse(new List<Message>());
            }
            else
            {
                var messages = _messages[_clientNumber];
                _messages[_clientNumber] = null;
                response = new MessagesResponse(messages);
            }

            SendReponse(response);
        }

        private void FileUploadHandler(IRequest request)
        {
            throw new NotImplementedException();
        }

        private void MessageHandler(IRequest request)
        {
            var messageRequest = (MessageRequest)request;
            SendReponse(new MessageResponse());

            if (messageRequest.ReceiversNumbers.Count() == 1)
            {
                var receiver = messageRequest.ReceiversNumbers.First();
                if(_messages[receiver] == null)
                {
                    _messages[receiver] = new List<Message>();
                }

                var message = new Message(_clientNumber, receiver, DateTime.UtcNow, messageRequest.Text);
                
                _messages[receiver].Add(message);
            }
            else if (messageRequest.ReceiversNumbers.Count() > 1)
            {
                // tu będzie inaczej, bo obsługa konferencji wymaga od nas albo
                // zmiany w message, albo innego typu message
                foreach (var receiver in messageRequest.ReceiversNumbers)
                {
                    if (_messages[receiver] == null)
                    {
                        _messages[receiver] = new List<Message>();
                    }

                    var message = new Message(_clientNumber, receiver, DateTime.UtcNow, messageRequest.Text);

                    _messages[receiver].Add(message);
                }
            }
                
            
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
