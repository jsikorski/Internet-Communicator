using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Messages
{
    [Serializable]
    public class ConferenceMessage
    {
        public int SenderNumber { get; private set; }
        public List<int> Collaborators { get; private set; }
        public DateTime Date { get; private set; }
        public string Content { get; private set; }

        public ConferenceMessage(int senderNumber, DateTime dateTime, string content, List<int> collaborators)
        {
            Collaborators = collaborators;
            SenderNumber = senderNumber;
            Date = dateTime;
            Content = content;
        }
    }
}
