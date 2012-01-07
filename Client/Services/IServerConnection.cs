using Protocol;
using Protocol.Login;
using Protocol.Registration;

namespace Client.Services
{
    public interface IServerConnection
    {
        void Connect(string serverAddress);
        LoginResponse SendLoginRequest(IRequest loginRequest);
        RegisterResponse SendRegisterRequest(IRequest registerRequest);
        void Disconnect();
    }
}
