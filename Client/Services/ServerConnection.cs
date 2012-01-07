using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Protocol;
using Protocol.Connection;
using Protocol.Login;
using Protocol.Register;

namespace Client.Services
{
    public class ServerConnection : IServerConnection
    {
        private TcpClient _server;

        private NetworkStream _serverStream;
        private NetworkStream _responsesStream;

        private readonly IFormatter _formatter;

        public ServerConnection()
        {
            _formatter = new BinaryFormatter();
        }
        
        public void Connect(string serverAddress)
        {
            var serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), Ports.ServerListeningPort);
            _server = new TcpClient();
            _server.Connect(serverEndPoint);
            _serverStream = _server.GetStream();
            SendRequest(new ConnectionRequest
                            {
                                ClientListeningAddress = Addresses.ClientListeningAddress,
                                ClientListenerPortNumber = Ports.ClientListeningPort
                            });

            var responsesListener = new TcpListener(
                IPAddress.Parse(Addresses.ClientListeningAddress), Ports.ClientListeningPort);
            responsesListener.Start();
            var serverAsSender = responsesListener.AcceptTcpClient();
            _responsesStream = serverAsSender.GetStream();
            responsesListener.Stop();
        }

        public LoginResponse SendLoginRequest(IRequest loginRequest)
        {
            SendRequest(loginRequest);
            return (LoginResponse) GetResponse();
        }

        public RegisterResponse SendRegisterRequest(IRequest registerRequest)
        {
            SendRequest(registerRequest);
            return (RegisterResponse) GetResponse();
        }

        public void Disconnect()
        {
            _server.Close();
        }

        private void SendRequest(IRequest request)
        {
            _formatter.Serialize(_serverStream, request);
        }

        private IResponse GetResponse()
        {
            var response = (IResponse) _formatter.Deserialize(_responsesStream);
            return response;
        }
    }
}
