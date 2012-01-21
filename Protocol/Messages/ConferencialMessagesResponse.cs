using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Messages;

namespace Protocol.Messages
{
    [Serializable]
    public class ConferencialMessagesResponse : IResponse
    {
        public IEnumerable<ConferencialMessage> Messages { get; private set; }

        public ConferencialMessagesResponse(IEnumerable<ConferencialMessage> messages)
        {
            Messages = messages;
        }
    }
}
