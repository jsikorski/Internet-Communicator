using Client.Context;
using Client.Features.Messages;
using Client.Services;
using Protocol.Messages;

namespace Client.Commands.Messages
{
    public class SendMessage : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly MessageData _messageData;
        private readonly ICurrentContext _currentContext;

        public SendMessage(
            IServerConnection serverConnection,
            MessageData messageData,
            ICurrentContext currentContext)
        {
            _serverConnection = serverConnection;
            _messageData = messageData;
            _currentContext = currentContext;
        }

        public void Execute()
        {
            var request = new MessageRequest(_currentContext.LoggedUserNumber,
                                             _messageData.ReceiverNumber,
                                             _messageData.Content);

            _serverConnection.SendMessageRequest(request);
        }
    }
}
