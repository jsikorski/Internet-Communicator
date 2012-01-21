using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Context;
using Client.Features.Messages;
using Client.Services;
using Protocol.Messages;

namespace Client.Commands.Messages
{
    public class SendConferencialMessage : ICommand
    {
        private readonly ConferencialMessageData _conferencialMessageData;
        private readonly IServerConnection _serverConnection;
        private readonly ICurrentContext _currentContext;

        public SendConferencialMessage(
            ConferencialMessageData conferencialMessageData,
            IServerConnection serverConnection,
            ICurrentContext currentContext)
        {
            _conferencialMessageData = conferencialMessageData;
            _serverConnection = serverConnection;
            _currentContext = currentContext;
        }

        public void Execute()
        {
            var request = new ConferenceMessageRequest(_currentContext.LoggedUserNumber,
                                                       _conferencialMessageData.Content,
                                                       _conferencialMessageData.ReceiversNumbers.ToList());

            _serverConnection.SendConferencialMessageRequest(request);
        }
    }
}
