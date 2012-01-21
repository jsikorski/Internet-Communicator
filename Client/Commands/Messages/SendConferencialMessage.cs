using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Services;
using Protocol.Messages;

namespace Client.Commands.Messages
{
    public class SendConferencialMessage : ICommand
    {
        private readonly ConferenceMessageRequest _conferenceMessageRequest;
        private readonly IServerConnection _serverConnection;

        public SendConferencialMessage(
            ConferenceMessageRequest conferenceMessageRequest, 
            IServerConnection serverConnection)
        {
            _conferenceMessageRequest = conferenceMessageRequest;
            _serverConnection = serverConnection;
        }

        public void Execute()
        {
            _serverConnection.SendConferencialMessageRequest(_conferenceMessageRequest);
        }
    }
}
