using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Messages
{
    [Serializable]
    public class Message
    {
        public int SenderNumber { get; private set; }
        public int ReceiverNumber { get; private set; }
        public DateTime DateTime { get; private set; }
        public string Content { get; private set; }

        public Message(int senderNumber, int receiverNumber, 
            DateTime dateTime, string content)
        {
            SenderNumber = senderNumber;
            ReceiverNumber = receiverNumber;
            DateTime = dateTime;
            Content = content;
        }
    }
}
