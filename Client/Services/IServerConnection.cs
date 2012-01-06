using Protocol;
using Protocol.Login;

namespace Client.Services
{
    public interface IServerConnection
    {
        void Connect(string serverAddress);
        LoginResponse SendLoginRequest(LoginRequest loginRequest);
        void Disconnect();
    }
}
