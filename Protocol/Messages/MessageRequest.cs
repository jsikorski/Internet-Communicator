using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Messages
{
    [Serializable]
    public class MessageRequest : IRequest
    {
        public int Sender { get; private set; }
        public int ReciverNumber { get; private set; }
        public string Text { get; private set; }

        public MessageRequest(int sender, int receiverNumber, string text)
        {
            Text = text;
            ReciverNumber = receiverNumber;
            Sender = sender;
        }
    }
}
