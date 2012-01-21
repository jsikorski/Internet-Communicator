using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Messages
{
    [Serializable]
    public class ConferencialMessage
    {
        public int SenderNumber { get; private set; }
        public IEnumerable<int> Collaborators { get; private set; }
        public DateTime Date { get; private set; }
        public string Content { get; private set; }

        public ConferencialMessage(int senderNumber, DateTime dateTime, string content, IEnumerable<int> collaborators)
        {
            Collaborators = collaborators;
            SenderNumber = senderNumber;
            Date = dateTime;
            Content = content;
        }
    }
}
