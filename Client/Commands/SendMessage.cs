using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Services;
using Protocol.Messages;

namespace Client.Commands
{
    public class SendMessage : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly MessageRequest _messageRequest;

        public SendMessage(
            IServerConnection serverConnection,
            MessageRequest messageRequest)
        {
            _serverConnection = serverConnection;
            _messageRequest = messageRequest;
        }

        public void Execute()
        {
            _serverConnection.SendMessageRequest(_messageRequest);
        }
    }
}
