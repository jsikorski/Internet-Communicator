using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Messages;

namespace Protocol.Messages
{
    [Serializable]
    public class ConferenceMessagesResponse : IResponse
    {
        public IEnumerable<ConferenceMessage> Messages { get; private set; }

        public ConferenceMessagesResponse(IEnumerable<ConferenceMessage> messages)
        {
            Messages = messages;
        }
    }
}
