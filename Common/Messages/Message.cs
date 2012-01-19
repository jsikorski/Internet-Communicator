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
        public DateTime Date { get; private set; }
        public string Content { get; private set; }

        public Message(int senderNumber, DateTime dateTime, string content)
        {
            SenderNumber = senderNumber;
            Date = dateTime;
            Content = content;
        }
    }
}
