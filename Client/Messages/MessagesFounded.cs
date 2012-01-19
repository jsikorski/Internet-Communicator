using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Messages;

namespace Client.Messages
{
    public class MessagesFounded
    {
        public IEnumerable<Message> Messages { get; private set; }
        
        public MessagesFounded(IEnumerable<Message> messages)
        {
            Messages = messages;
        }

    }
}
