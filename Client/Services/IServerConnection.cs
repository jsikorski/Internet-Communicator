using System.Collections.Generic;
using Common.Contacts;
using Protocol;
using Protocol.FileTransfer;
using Protocol.Login;
using Protocol.Messages;
using Protocol.Register;
using Protocol.Statuses;

namespace Client.Services
{
    public interface IServerConnection
    {
        bool IsConnected { get; }

        void Connect(string serverAddress);
        LoginResponse SendLoginRequest(IRequest loginRequest);
        RegisterResponse SendRegisterRequest(IRequest registerRequest);
        StatusesResponse SendStatusesRequest(IRequest statusesRequest);
        MessageResponse SendMessageRequest(IRequest messageRequest);
        MessagesResponse SendMessagesRequest(IRequest messagesRequest);
        FileUploadResponse SendFileUploadRequest(IRequest uploadRequest);
        FilesDownloadResponse SendFilesDownloadRequest(IRequest filesDownloadRequest);
        FileDownloadResponse SendFileDownloadRequest(IRequest fileDownloadRequest);
        ConferencialMessageResponse SendConferencialMessageRequest(IRequest messageRequest);
        ConferencialMessagesResponse SendConferencialMessagesRequest(IRequest messagesRequest);
        void Disconnect();
    }
}
