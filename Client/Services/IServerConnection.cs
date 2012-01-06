using Protocol;
using Protocol.AccountCreation;
using Protocol.Login;

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
