using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Messages;

namespace Protocol.Messages
{
    [Serializable]
    public class MessagesResponse : IResponse
    {
        public IEnumerable<Message> Messages { get; private set; }

        public MessagesResponse(IEnumerable<Message> messages)
        {
            Messages = messages;
        }
    }
}
