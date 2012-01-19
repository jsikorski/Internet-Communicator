using System.Collections.Generic;
using Common.Contacts;
using Protocol;
using Protocol.Login;
using Protocol.Messages;
using Protocol.Register;
using Protocol.Statuses;

namespace Client.Services
{
    public interface IServerConnection
    {
        void Connect(string serverAddress);
        LoginResponse SendLoginRequest(IRequest loginRequest);
        RegisterResponse SendRegisterRequest(IRequest registerRequest);
        StatusesResponse SendStatusesRequest(IRequest statusesRequest);
        MessagesResponse SendMessagesRequest(IRequest messagesRequest);
        void Disconnect();
    }
}
