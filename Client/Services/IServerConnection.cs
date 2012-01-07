using Protocol;
using Protocol.Login;
using Protocol.Register;

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
