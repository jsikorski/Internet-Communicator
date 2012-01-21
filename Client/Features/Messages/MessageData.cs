using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Features.Messages
{
    public class MessageData
    {
        public int ReceiverNumber { get; private set; }
        public string Content { get; private set; }

        public MessageData(int receiverNumber, string content)
        {
            Content = content;
            ReceiverNumber = receiverNumber;
        }
    }
}
