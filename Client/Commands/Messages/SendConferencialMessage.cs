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
            IEnumerable<int> collaboratorsNumbers = _conferencialMessageData
                .ReceiversNumbers.Concat(new List<int> {_currentContext.LoggedUserNumber}).ToList();
            var request = new ConferencialMessageRequest(_currentContext.LoggedUserNumber,
                                                       _conferencialMessageData.Content,
                                                       collaboratorsNumbers);

            _serverConnection.SendConferencialMessageRequest(request);
        }
    }
}
